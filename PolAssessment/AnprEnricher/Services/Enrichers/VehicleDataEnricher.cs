using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

internal class VehicleDataEnricher(ILogger<VehicleDataEnricher> logger) : IEnricher
{
    private readonly ILogger<VehicleDataEnricher> _logger = logger;

    public EnrichedBaseData Enrich(AnprData data)
    {
        _logger.LogInformation("Enriching vehicle data.");

        return new EnrichedVehicleData
        {
            TechnicalName = "TechnicalName",
            BrandName = "BrandName",
            ApkExpirationDate = DateTime.Now
        };
    }
}
