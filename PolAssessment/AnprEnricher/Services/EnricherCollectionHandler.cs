using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;
using PolAssessment.AnprEnricher.Services.Enrichers;

namespace PolAssessment.AnprEnricher.Services;

public interface IEnricherCollection
{
    event EventHandler<EnricherCollectionHandler.EnrichedDataEventArgs> FinishedEnrichedData;
}

public class EnricherCollectionHandler : IEnricherCollection
{
    private readonly ILogger<EnricherCollectionHandler> _logger;
    private readonly IEnumerable<IEnricher> _enrichers;

    public EnricherCollectionHandler(ILogger<EnricherCollectionHandler> logger, IEnumerable<IEnricher> enrichers, IFileHandler anprHandler)
    {
        _logger = logger;
        _enrichers = enrichers;
        anprHandler.NewAnprDataRead += AnprHandler_NewAnprDataRead;
    }

    public event EventHandler<EnrichedDataEventArgs>? FinishedEnrichedData;

    private void OnFinishedEnrichedData(EnrichedDataEventArgs e)
    {
        _logger.LogInformation("Finished enriching ANPR data.");
        FinishedEnrichedData?.Invoke(this, e);
    }

    private async void AnprHandler_NewAnprDataRead(object? sender, FileHandler.AnprDataEventArgs e)
    {
        await EnrichAnprData(e.Data);
    }

    public async Task EnrichAnprData(AnprData data)
    {
        var enrichedDataCollection = new Dictionary<string, object>
        {
            { nameof(AnprData), data }
        };

        foreach (var enricher in _enrichers)
        {
            var enrichedData = await enricher.Enrich(data);
            enrichedDataCollection.Add(enrichedData.GetType().Name, enrichedData);
        }
        OnFinishedEnrichedData(new EnrichedDataEventArgs(enrichedDataCollection));
    }

    public class EnrichedDataEventArgs(Dictionary<string, object> enrichedData) : EventArgs
    {
        public Dictionary<string, object> EnrichedData { get; } = enrichedData;
    }
}
