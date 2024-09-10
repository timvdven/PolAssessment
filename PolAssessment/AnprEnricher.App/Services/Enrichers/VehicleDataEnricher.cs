using AutoMapper;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.App.Models;
using PolAssessment.AnprEnricher.App.Models.Dto;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.App.Services.Enrichers;

internal class VehicleDataEnricher(ILogger<VehicleDataEnricher> logger, IVehicleDataUrlService vehicleDataUrlService, HttpClient httpClient, IMapper mapper) : BaseHttpClientEnricher(httpClient), IEnricher
{
    private readonly ILogger<VehicleDataEnricher> _logger = logger;
    private readonly IVehicleDataUrlService _vehicleDataUrlService = vehicleDataUrlService;
    private readonly IMapper _mapper = mapper;

    public async Task<object> Enrich(AnprData data)
    {
        _logger.LogInformation("Enriching vehicle data.");

        var licensePlate = GetLicensePlateData(data.Plate);
        var vehicleEnricherUrl = _vehicleDataUrlService.GetVehicleDataUrl(licensePlate);

        _logger.LogInformation("Enriching vehicle data for license plate {licensePlate} using URL {vehicleEnricherUrl}.", licensePlate, vehicleEnricherUrl);
        var rawString = await GetDataFromApiAsync(vehicleEnricherUrl);

        _logger.LogInformation("Deserializing vehicle data.");
        var vehicleDataDtoResponse = JsonSerializer.Deserialize<IEnumerable<VehicleDataDto.Properties>>(rawString)?.FirstOrDefault() ?? throw new JsonException();

        _logger.LogInformation("Mapping vehicle data.");
        var result = _mapper.Map<EnrichedVehicleData>(vehicleDataDtoResponse);

        _logger.LogInformation("Data enriched with Vehicle data.");
        return result;
    }

    /// <summary>
    /// Returns the license plate data in a clean format without any dashes.
    /// </summary>
    private string GetLicensePlateData(string licensePlate)
    {
        _logger.LogInformation("Cleaning up license plate.");
        return licensePlate.Replace("-", "").Replace(" ", "").ToUpper();
    }
}
