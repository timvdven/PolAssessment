﻿using System.Text.Json.Serialization;

namespace PolAssessment.AnprEnricher.App.Models;

public class EnrichedVehicleData
{
    [JsonPropertyName("technicalName")]
    public string? TechnicalName { get; set; }

    [JsonPropertyName("brandName")]
    public string? BrandName { get; set; }

    [JsonPropertyName("apkExpirationDate")]
    public DateTime? ApkExpirationDate { get; set; }
}
