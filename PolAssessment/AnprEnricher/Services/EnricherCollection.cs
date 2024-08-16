using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;
using PolAssessment.AnprEnricher.Services.Enrichers;

namespace PolAssessment.AnprEnricher.Services;

public interface IEnricherCollection
{
    event EventHandler<EnricherCollection.EnrichedDataEventArgs> FinishedEnrichedData;
}

public class EnricherCollection : IEnricherCollection
{
    private readonly ILogger<EnricherCollection> _logger;
    private readonly IEnumerable<IEnricher> _enrichers;

    public EnricherCollection(ILogger<EnricherCollection> logger, IEnumerable<IEnricher> enrichers, IAnprHandler anprHandler)
    {
        _logger = logger;
        _enrichers = enrichers;
        anprHandler.NewAnprDataRead += AnprHandler_NewAnprDataRead;
    }

    public event EventHandler<EnrichedDataEventArgs>? FinishedEnrichedData;

    protected virtual void OnFinishedEnrichedData(EnrichedDataEventArgs e)
    {
        _logger.LogInformation("Finished enriching ANPR data.");
        FinishedEnrichedData?.Invoke(this, e);
    }

    private async void AnprHandler_NewAnprDataRead(object? sender, AnprHandler.AnprEventArgs e)
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
