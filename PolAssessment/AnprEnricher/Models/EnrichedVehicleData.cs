using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.Models;

public class EnrichedVehicleData : EnrichedBaseData
{
    [JsonPropertyName("licensePlate")]
    public string? TechnicalName { get; set; }

    [JsonPropertyName("brandName")]
    public string? BrandName { get; set; }

    [JsonPropertyName("apkExpirationDate")]
    public DateTime? ApkExpirationDate { get; set; }
}
