using AutoMapper;
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
    private readonly IDataSendThreadControlService _dataSendThreadControlService;

    public AnprEnrichedDataSender(ILogger<AnprEnrichedDataSender> logger, IMapper mapper, IEnricherCollection enrichers, HttpClient httpClient, ITokenService tokenService, IOptions<AnprDataProcessorConfig> config, IDataSendThreadControlService dataSendThreadControlService)
    {
        _logger = logger;
        _logger.LogInformation("AnprEnrichedDataSender created, starting service...");
        _httpClient = httpClient;
        _config = config.Value;
        _mapper = mapper;
        _tokenService = tokenService;
        _dataSendThreadControlService = dataSendThreadControlService;
        enrichers.FinishedEnrichedData += EnricherCollection_FinishedEnrichedData;
    }

    private async void EnricherCollection_FinishedEnrichedData(object? sender, EnricherCollectionHandler.EnrichedDataEventArgs e)
    {
        await _dataSendThreadControlService.WaitAsync();
        try
        {
            await HandleEnrichedCollection(e);
        }
        finally
        {
            _dataSendThreadControlService.Release();
        }
    }

    private async Task HandleEnrichedCollection(EnricherCollectionHandler.EnrichedDataEventArgs e)
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

    private async Task SendData(string body, int attempt = 1)
    {
        if (string.IsNullOrEmpty(body))
        {
            _logger.LogWarning("Empty body. Skipping sending data.");
            return;
        }

        try
        {
            var url = string.Concat(_config.BaseUrl, _config.Operation.Anpr);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var token = await _tokenService.GetTokenAsync() ?? throw new InvalidOperationException("SendData failed to get token.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            if (_httpClient == null || url == null || content == null)
            {
                _logger.LogError("HttpClient, url, or content is null.");
            }

            var response = await _httpClient!.PostAsync(url, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogError("Unauthorized. Please check token: {token}.", token.Token);
                throw new AuthenticationException("Unauthorized. Please check the token.");
            }
        }
        catch(Exception ex) when (attempt < _config.MaxRetries)
        {
            _logger.LogError(ex, "Failed to send data at attempt {attempt}. Retrying in {retryDelay}...", attempt, _config.RetryDelay);
            Thread.Sleep(_config.RetryDelay);

            await SendData(body, attempt + 1);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to send data.");
            throw;
        }
    }
}
