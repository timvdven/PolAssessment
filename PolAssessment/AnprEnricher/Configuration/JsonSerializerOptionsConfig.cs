using System.Text.Json;

namespace PolAssessment.AnprEnricher.Configuration
{
    public class JsonSerializerOptionsConfig
    {
        public JsonSerializerOptions Options { get; } = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}
