using PolAssessment.AnprEnricher.App.Models;

namespace PolAssessment.AnprEnricher.App.Services.Enrichers;

public interface IEnricher
{
    Task<object> Enrich(AnprData data);
}
