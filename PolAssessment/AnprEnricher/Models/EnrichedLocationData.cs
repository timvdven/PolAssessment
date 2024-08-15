using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.Models;

[JsonConverter(typeof(EnrichedLocationData))]
public class EnrichedLocationData : EnrichedBaseData
{
    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }
}
