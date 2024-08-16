using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions;
using PolAssessment.AnprEnricher.Models;
using PolAssessment.AnprEnricher.Models.Dto;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

internal class VehicleDataEnricher(ILogger<VehicleDataEnricher> logger, IConfiguration config, HttpClient httpClient, IMapper mapper) : BaseHttpClientEnricher(httpClient), IEnricher
{
    private readonly ILogger<VehicleDataEnricher> _logger = logger;
    private readonly IConfiguration _config = config;
    private readonly IMapper _mapper = mapper;

    public async Task<object> Enrich(AnprData data)
    {
        _logger.LogInformation("Enriching vehicle data.");

        var licensePlate = CleanUpLicensePlate(data.Plate);
        var vehicleEnricherUrl = _config.GetVehicleEnricherUrl(licensePlate);

        _logger.LogInformation("Enriching vehicle data for license plate {licensePlate} using URL {vehicleEnricherUrl}.", licensePlate, vehicleEnricherUrl);
        var rawString = await GetDataFromApiAsync(vehicleEnricherUrl);

        _logger.LogInformation("Deserializing vehicle data.");
        var vehicleDataDtoResponse = JsonSerializer.Deserialize<IEnumerable<VehicleDataDto.Properties>>(rawString)?.FirstOrDefault() ?? throw new JsonException();

        _logger.LogInformation("Mapping vehicle data.");
        var result = _mapper.Map<EnrichedVehicleData>(vehicleDataDtoResponse);

        _logger.LogInformation("Data enriched with Vehicle data.");
        return result;
    }

    private string CleanUpLicensePlate(string licensePlate)
    {
        _logger.LogInformation("Cleaning up license plate.");
        return licensePlate.Replace("-", "").Replace(" ", "").ToUpper();
    }
}
