using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions.ConfigurationExtensions;
using PolAssessment.AnprEnricher.Models;
using PolAssessment.AnprEnricher.Models.Dto;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

internal class LocationDataEnricher(ILogger<LocationDataEnricher> logger, IConfiguration configuration, HttpClient httpClient, IMapper mapper) : BaseHttpClientEnricher(httpClient), IEnricher
{
    private readonly ILogger<LocationDataEnricher> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMapper _mapper = mapper;

    public async Task<object> Enrich(AnprData data)
    {
        _logger.LogInformation("Enriching location data.");

        var url = _configuration.GetLocationEnricherUrl(data.Coordinates.Latitude, data.Coordinates.Longitude);

        _logger.LogInformation("Enriching location data for URL {url}.", url);
        var rawString = await GetDataFromApiAsync(url);

        _logger.LogInformation("Deserializing location data.");
        var locationDataDtoResponse = JsonSerializer.Deserialize<LocationDataDto.Response>(rawString) ?? throw new JsonException();
        var locationDataProperties = locationDataDtoResponse.Features?.FirstOrDefault(x => x.Properties != null)?.Properties ?? throw new JsonException("Can't extract Location Properties.");

        _logger.LogInformation("Mapping location data.");
        var result = _mapper.Map<EnrichedLocationData>(locationDataProperties);

        _logger.LogInformation("Data enriched with Location data.");
        return result;
    }
}
