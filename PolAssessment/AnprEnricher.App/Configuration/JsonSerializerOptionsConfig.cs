using System.Text.Json;

namespace PolAssessment.AnprEnricher.App.Configuration;

public class JsonSerializerOptionsConfig
{
    public JsonSerializerOptions Options { get; } = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
}
