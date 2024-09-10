namespace PolAssessment.AnprEnricher.App.Configuration;

public class LocationEnricherConfig
{
    public required string BaseUrl { get; set; }
    public required QueryParametersConfig QueryParameters { get; set; }
    public required string ApiKey { get; set; }

    public class QueryParametersConfig
    {
        public required string Latitude { get; set; }
        public required string Longitude { get; set; }
        public required string ApiKey { get; set; }
    }
}
