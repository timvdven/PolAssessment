using System.Collections.Specialized;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;

namespace PolAssessment.AnprEnricher.Services;

public interface ILocationDataUrlService
{
    string GetLocationDataUrl(double latitude, double longitude);
}

public class LocationDataUrlService(IOptions<LocationEnricherConfig> config) : ILocationDataUrlService
{
    private readonly LocationEnricherConfig _config = config.Value;

    public string GetLocationDataUrl(double latitude, double longitude)
    {
        var apiKey = _config.ApiKey;
        var queryParameters = _config.QueryParameters;
        var baseUrl = _config.BaseUrl;

        var uriBuilder = new UriBuilder(baseUrl);
        var query = new NameValueCollection
        {
            [queryParameters.Latitude] = latitude.ToString(),
            [queryParameters.Longitude] = longitude.ToString(),
            [queryParameters.ApiKey] = apiKey
        };
        uriBuilder.Query = query.ToString();

        return uriBuilder.ToString();
    }
}
