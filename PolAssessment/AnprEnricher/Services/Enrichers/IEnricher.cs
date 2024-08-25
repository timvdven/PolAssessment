using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

public interface IEnricher
{
    Task<object> Enrich(AnprData data);
}
