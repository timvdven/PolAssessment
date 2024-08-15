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

    public IEnumerable<EnrichedBaseData> EnrichedData { get; set; } = [];

    protected virtual void OnFinishedEnrichedData(EnrichedDataEventArgs e)
    {
        _logger.LogInformation("Finished enriching ANPR data.");
        FinishedEnrichedData?.Invoke(this, e);
    }

    private void AnprHandler_NewAnprDataRead(object? sender, AnprHandler.AnprEventArgs e)
    {
        EnrichedData = Enrich(e.Data);
        OnFinishedEnrichedData(new EnrichedDataEventArgs(EnrichedData));
    }

    public IEnumerable<EnrichedBaseData> Enrich(AnprData data)
    {
        var result = new List<EnrichedBaseData>();
        foreach (var enricher in _enrichers)
        {
            var enrichedData = enricher.Enrich(data);
            result.Add(enrichedData);
        }
        return result;
    }

    public class EnrichedDataEventArgs(IEnumerable<EnrichedBaseData> enrichedData) : EventArgs
    {
        public IEnumerable<EnrichedBaseData> EnrichedData { get; } = enrichedData;
    }
}
