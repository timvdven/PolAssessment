using AutoMapper;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;
using PolAssessment.AnprEnricher.Models.Dto;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

internal class LocationDataEnricher(ILogger<LocationDataEnricher> logger, ILocationDataUrlService locationDataUrlService, HttpClient httpClient, IMapper mapper) : BaseHttpClientEnricher(httpClient), IEnricher
{
    private readonly ILogger<LocationDataEnricher> _logger = logger;
    private readonly ILocationDataUrlService _locationDataUrlService = locationDataUrlService;
    private readonly IMapper _mapper = mapper;

    public async Task<object> Enrich(AnprData data)
    {
        _logger.LogInformation("Enriching location data.");

        var url = _locationDataUrlService.GetLocationDataUrl(data.Coordinates.Latitude, data.Coordinates.Longitude);

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
