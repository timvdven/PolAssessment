namespace PolAssessment.AnprEnricher.Configuration;

public class LocationEnricherConfig
{
    public required string BaseUrl { get; set; }
    public required QueryParametersSubclass QueryParameters { get; set; }
    public required string ApiKey { get; set; }

    public class QueryParametersSubclass
    {
        public required string Latitude { internal get; set; }
        public required string Longitude { internal get; set; }
        public required string ApiKey { internal get; set; }
    }
}