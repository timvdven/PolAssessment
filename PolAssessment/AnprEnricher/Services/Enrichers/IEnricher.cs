using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

public interface IEnricher
{
    EnrichedBaseData Enrich(AnprData data);
}
