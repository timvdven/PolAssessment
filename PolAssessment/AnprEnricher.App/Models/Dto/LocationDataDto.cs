using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.App.Models.Dto;

public class LocationDataDto
{
    public class Response
    {
        [JsonPropertyName("features")]
        public IEnumerable<Feature>? Features { get; set; }
    }

    public class Feature
    {
        [JsonPropertyName("properties")]
        public Properties? Properties { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }
    }
}
