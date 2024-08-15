using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

internal class AnprEnrichedDataSender
{
    private readonly ILogger<AnprEnrichedDataSender> _logger;

    public AnprEnrichedDataSender(ILogger<AnprEnrichedDataSender> logger, IEnricherCollection enricherCollection)
    {
        _logger = logger;
        enricherCollection.FinishedEnrichedData += EnricherCollection_FinishedEnrichedData;
    }

    private void EnricherCollection_FinishedEnrichedData(object? sender, EnricherCollection.EnrichedDataEventArgs e)
    {
        var body = JsonSerializer.Serialize(e.EnrichedData);
        _logger.LogInformation("Sending enriched data: {body}", body);
    }
}
