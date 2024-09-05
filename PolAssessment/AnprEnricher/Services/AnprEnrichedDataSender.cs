using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;
using PolAssessment.Shared.Models;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

internal class AnprEnrichedDataSender
{
    private readonly ILogger<AnprEnrichedDataSender> _logger;
    private readonly HttpClient _httpClient;
    private readonly AnprDataProcessorConfig _config;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public AnprEnrichedDataSender(ILogger<AnprEnrichedDataSender> logger, IMapper mapper, IEnricherCollection enrichers, HttpClient httpClient, ITokenService tokenService, IOptions<AnprDataProcessorConfig> config)
    {
        _logger = logger;
        _logger.LogInformation("AnprEnrichedDataSender created, starting service...");
        _httpClient = httpClient;
        _config = config.Value;
        _mapper = mapper;
        _tokenService = tokenService;
        enrichers.FinishedEnrichedData += EnricherCollection_FinishedEnrichedData;
    }

    private async void EnricherCollection_FinishedEnrichedData(object? sender, EnricherCollectionHandler.EnrichedDataEventArgs e)
    {
        _logger.LogInformation("Enriched data received. Going to map and send data...");
        var anprRecord = MapToAnprRecord(e.EnrichedData);
        var body = JsonSerializer.Serialize(anprRecord);

        _logger.LogInformation("Sending enriched data: {body}", body);
        await SendData(body);
        _logger.LogInformation("Enriched data sent successfully.");
    }

    public AnprRecord MapToAnprRecord(Dictionary<string, object> keyValues)
    {
        var anprRecord = new AnprRecord { LicensePlate = String.Empty };
        foreach(var kvp in keyValues)
        {
            _mapper.Map(kvp.Value, anprRecord);
        }
        return anprRecord;
    }

    private async Task SendData(string body)
    {
        var url = string.Concat(_config.BaseUrl, _config.Operation.Anpr);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var token = await _tokenService.GetTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await _httpClient.PostAsync(url, content);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new AuthenticationException("Unauthorized. Please check the token.");
            // if (isRetryAfterFetchToken)
            // {
            //     _logger.LogError("Failed to send enriched data. Status code: {statusCode}", response.StatusCode);
            // }
            // else
            // {
            //     // Make sure only one thread is fetching token, then wait at least 5 minutes before fetching again
            //     lock (_lock)
            //     {
            //         if (!lastTokenFetchTime.HasValue || DateTime.Now - lastTokenFetchTime > TimeSpan.FromMinutes(5))
            //         {
            //             token = FetchToken();
            //             lastTokenFetchTime = DateTime.Now;
            //         }
            //     }
            //     await SendData(body, true);
            // }
        }
    }

    // private async Task RetrySendData(string body)
    // {
    //     _logger.LogInformation("Retrying after fetching token...");
    //     token = FetchToken();
    //     await SendData(body, true);
    // }

    // private string FetchToken()
    // {
    //     var url = _configuration.GetAnprDataProcessorAuthorizeUrl();
    //     var clientId = _configuration.GetAnprDataProcessorClientId();
    //     var clientSecret = _configuration.GetAnprDataProcessorClientSecret();

    //     _httpClient.DefaultRequestHeaders.Add("Client-Id", clientId);
    //     _httpClient.DefaultRequestHeaders.Add("Client-Secret", clientSecret);
        
    //     _logger.LogInformation("Fetching token from {url}", url);
    //     var response = _httpClient.GetAsync(url).Result;

    //     var responseBody = response.Content.ReadAsStringAsync().Result;
    //     if (response.IsSuccessStatusCode)
    //     {
    //         _logger.LogInformation("Token fetched successfully.");
    //         var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody);
    //         return tokenResponse?.Token ?? throw new AuthenticationException("Access token not found in response.");
    //     }
    //     else
    //     {
    //         _logger.LogError("Failed to fetch token: {id}-{secret}: {msg}", clientId, clientSecret, responseBody);
    //         throw new AuthenticationException($"Failed to fetch token. Status code: {response.StatusCode}");
    //     }
    // }
}
