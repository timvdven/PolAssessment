using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

internal class AnprEnrichedDataSender
{
    private readonly ILogger<AnprEnrichedDataSender> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private static string? token;

    public AnprEnrichedDataSender(ILogger<AnprEnrichedDataSender> logger, IEnricherCollection enrichers, HttpClient httpClient, IConfiguration configuration)
    {
        _logger = logger;
        _logger.LogInformation("AnprEnrichedDataSender created, starting service...");
        _httpClient = httpClient;
        _configuration = configuration;
        enrichers.FinishedEnrichedData += EnricherCollection_FinishedEnrichedData;
    }

    private void EnricherCollection_FinishedEnrichedData(object? sender, EnricherCollection.EnrichedDataEventArgs e)
    {
        // TODO: Send enriched data to the next step in the pipeline

        var body = JsonSerializer.Serialize(e.EnrichedData);

        _logger.LogInformation("Sending enriched data: {body}", body);
    }

    private string GetToken()
    {
        if (token is not null)
        {
            return token;
        }
}
