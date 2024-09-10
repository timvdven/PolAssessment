using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.App.Configuration;
using PolAssessment.Common.Lib.Models;
using PolAssessment.Common.Lib.Models.DataProcessor;

namespace PolAssessment.AnprEnricher.App.Services;

public interface ITokenService
{
    Task<AccessToken> GetTokenAsync();
}

public class TokenService(HttpClient httpClient, ILogger<TokenService> logger, IOptions<AnprDataProcessorConfig> config, JsonSerializerOptionsConfig jsonSerializerOptionsConfig) : ITokenService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<TokenService> _logger = logger;
    private readonly AnprDataProcessorConfig _config = config.Value;
    private readonly JsonSerializerOptionsConfig _jsonSerializerOptionsConfig = jsonSerializerOptionsConfig;
    private AccessToken? _token;
    private readonly object _lock = new();

    private async Task<AccessToken> FetchTokenAsync()
    {
        var authorizeRequest = new AuthorizeRequest
        {
            ClientId = _config.ClientId,
            ClientSecret = _config.ClientSecret
        };

        var url = string.Concat(_config.BaseUrl, _config.Operation.Authorize);
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(authorizeRequest), Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get token. Status code: {StatusCode}", response.StatusCode);
            throw new InvalidOperationException("Failed to get token");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var authorizeResponse = DeserializeAuthorizeResponse(responseContent);

        return authorizeResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get access token");
    }

    public AuthorizeResponse DeserializeAuthorizeResponse(string responseContent)
    {
        _logger.LogInformation("Deserializing token response...");
        return JsonSerializer.Deserialize<AuthorizeResponse>(responseContent, _jsonSerializerOptionsConfig.Options)
            ?? throw new InvalidOperationException("Failed to deserialize token response");
    }

    public async Task<AccessToken> GetTokenAsync()
    {
        return await GetTokenAsync(1);
    }

    public async Task<AccessToken> GetTokenAsync(int attempt)
    {
        if (_token != null && !ValidateToken(_token))
        {
            _token = null;
        }

        try
        {
            lock (_lock)
            {
                _token ??= FetchTokenAsync().Result;
            }

            return _token;
        }
        catch (InvalidOperationException ex) when (attempt < _config.MaxRetries)
        {
            _logger.LogError(ex, "Failed to get token at attempt {attempt}. Retrying...", attempt);

            Thread.Sleep(_config.RetryDelay);
            _token = await GetTokenAsync(attempt + 1);
            return _token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get token");
            throw;
        }
    }

    private bool ValidateToken(AccessToken token)
    {
        _logger.LogInformation("Validating token...");
        return token.Expiry > DateTime.UtcNow;
    }
}
