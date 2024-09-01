using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions.ConfigurationExtensions;
using PolAssessment.Shared.Models;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.Services;

internal class AnprEnrichedDataSender
{
    private readonly ILogger<AnprEnrichedDataSender> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private static string? token;
    private static DateTime? lastTokenFetchTime;
    private object _lock = new();

    public AnprEnrichedDataSender(ILogger<AnprEnrichedDataSender> logger, IMapper mapper, IEnricherCollection enrichers, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _logger.LogInformation("AnprEnrichedDataSender created, starting service...");
        _httpClient = httpClient;
        _configuration = configuration;
        _mapper = mapper;
        enrichers.FinishedEnrichedData += EnricherCollection_FinishedEnrichedData;
    }

    private async void EnricherCollection_FinishedEnrichedData(object? sender, EnricherCollectionHandler.EnrichedDataEventArgs e)
    {
        var anprRecord = new AnprRecord { LicensePlate = String.Empty };
        foreach(var kvp in e.EnrichedData)
        {
            _mapper.Map(kvp.Value, anprRecord);
        }

        var body = JsonSerializer.Serialize(anprRecord);

        _logger.LogInformation("Sending enriched data: {body}", body);
        await SendData(body);
        _logger.LogInformation("Enriched data sent successfully.");
    }

    private async Task SendData(string body, bool isRetryAfterFetchToken = false)
    {
        var url = _configuration.GetAnprDataProcessorAnprUrl();
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsync(url, content);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if (isRetryAfterFetchToken)
            {
                _logger.LogError("Failed to send enriched data. Status code: {statusCode}", response.StatusCode);
            }
            else
            {
                // Make sure only one thread is fetching token, then wait at least 5 minutes before fetching again
                lock (_lock)
                {
                    if (!lastTokenFetchTime.HasValue || DateTime.Now - lastTokenFetchTime > TimeSpan.FromMinutes(5))
                    {
                        token = FetchToken();
                        lastTokenFetchTime = DateTime.Now;
                    }
                }
                await SendData(body, true);
            }
        }
    }

    private async Task RetrySendData(string body)
    {
        _logger.LogInformation("Retrying after fetching token...");
        token = FetchToken();
        await SendData(body, true);
    }

    private string FetchToken()
    {
        var url = _configuration.GetAnprDataProcessorAuthorizeUrl();
        var clientId = _configuration.GetAnprDataProcessorClientId();
        var clientSecret = _configuration.GetAnprDataProcessorClientSecret();

        _httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);
        _httpClient.DefaultRequestHeaders.Add("Client-Secret", clientSecret);
        
        _logger.LogInformation("Fetching token from {url}", url);
        var response = _httpClient.GetAsync(url).Result;

        var responseBody = response.Content.ReadAsStringAsync().Result;
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Token fetched successfully.");
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody);
            return tokenResponse?.Token ?? throw new AuthenticationException("Access token not found in response.");
        }
        else
        {
            _logger.LogError("Failed to fetch token: {id}-{secret}: {msg}", clientId, clientSecret, responseBody);
            throw new AuthenticationException($"Failed to fetch token. Status code: {response.StatusCode}");
        }
    }
}

internal class TokenResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
