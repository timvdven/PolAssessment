using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.Models;

public class EnrichedLocationData
{
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }
}
