using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;
using PolAssessment.Shared.Models;
using PolAssessment.Shared.Models.DataProcessor;

namespace PolAssessment.AnprEnricher.Services;

public interface ITokenService
{
    Task<AccessToken> GetTokenAsync();
}

public class TokenService(HttpClient httpClient, ILogger<TokenService> logger, IOptions<AnprDataProcessorConfig> config) : ITokenService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<TokenService> _logger = logger;
    private readonly AnprDataProcessorConfig _config = config.Value;
    private AccessToken? _token;
    private DateTime? _lastTokenFetchTime;

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
        var authorizeResponse = JsonSerializer.Deserialize<AuthorizeResponse>(responseContent) 
            ?? throw new InvalidOperationException("Failed to deserialize token response");

        return authorizeResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get access token");
    }

    public async Task<AccessToken> GetTokenAsync()
    {
        int maxRetries = _config.MaxRetries;
        int retryDelay = _config.RetryDelay;
        int attempts = 0;

        if (_token != null && !ValidateToken(_token))
        {
            _token = null;
        }

        try
        {
            if (_token == null)
            {
                _token = await FetchTokenAsync();
                _lastTokenFetchTime = DateTime.Now;
            }

            return _token;
        }
        catch (InvalidOperationException ex) when (attempts < maxRetries)
        {
            _logger.LogError(ex, "Failed to get token. Retrying...");
            attempts++;
            Thread.Sleep(retryDelay);
            _token = await GetTokenAsync();
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
        return token.Expiry > DateTime.Now;
    }
}
