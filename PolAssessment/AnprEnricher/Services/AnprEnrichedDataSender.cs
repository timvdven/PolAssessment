using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

internal class AnprEnrichedDataSender
{
    private readonly ILogger<AnprEnrichedDataSender> _logger;

    public AnprEnrichedDataSender(ILogger<AnprEnrichedDataSender> logger, IEnricherCollection enrichers)
    {
        _logger = logger;
        enrichers.FinishedEnrichedData += EnricherCollection_FinishedEnrichedData;
    }

    private void EnricherCollection_FinishedEnrichedData(object? sender, EnricherCollection.EnrichedDataEventArgs e)
    {
        // TODO: Send enriched data to the next step in the pipeline

        var body = JsonSerializer.Serialize(e.EnrichedData);

        _logger.LogInformation("Sending enriched data: {body}", body);
    }
}
