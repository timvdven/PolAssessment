using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

internal class LocationDataEnricher(ILogger<LocationDataEnricher> logger) : IEnricher
{
    private readonly ILogger<LocationDataEnricher> _logger = logger;

    public EnrichedBaseData Enrich(AnprData data)
    {
        _logger.LogInformation("Enriching location data."); 
        return new EnrichedLocationData
        {
            Street = "Street",
            City = "City"
        };
    }
}
