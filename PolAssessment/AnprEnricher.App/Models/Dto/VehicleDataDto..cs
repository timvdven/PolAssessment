using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.App.Models.Dto;

public class VehicleDataDto
{
    public class Properties
    {
        [JsonPropertyName("kenteken")]
        public required string LicensePlate { get; set; }

        [JsonPropertyName("handelsbenaming")]
        public string? TechnicalName { get; set; }

        [JsonPropertyName("merk")]
        public string? BrandName { get; set; }

        [JsonPropertyName("vervaldatum_apk")]
        public string? ApkExpirationDate { get; set; }
    }
}
