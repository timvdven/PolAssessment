using System.Web;
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
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString[queryParameters.Latitude] = latitude.ToString();
        queryString[queryParameters.Longitude] = longitude.ToString();
        queryString[queryParameters.ApiKey] = apiKey;

        uriBuilder.Query = queryString.ToString()!.Replace("%2C", ".", StringComparison.InvariantCultureIgnoreCase);

        return uriBuilder.ToString();
    }
}
