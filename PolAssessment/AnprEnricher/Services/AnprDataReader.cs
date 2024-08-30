using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions.ConfigurationExtensions;
using PolAssessment.AnprEnricher.Models;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

public interface IAnprDataReader
{
    AnprData ReadAnprData(string fullPath);
}

public class AnprDataReader(ILogger<AnprDataReader> logger, IConfiguration configuration, IFileService fileReader) : IAnprDataReader
{
    private readonly ILogger<AnprDataReader> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IFileService _fileReader = fileReader;

    public AnprData ReadAnprData(string fullPath)
    {
        try
        {
            var rawString = _fileReader.ReadAllText(fullPath, _configuration.GetMaxRetriesForReadingFile());
            var result = JsonSerializer.Deserialize<AnprData>(rawString);
            return result ?? throw new JsonException("Could not deserialize the ANPR data.");
        }
        catch(JsonException ex)
        {
            _logger.LogError(ex, "Could not deserialize the ANPR data.");
            throw;
        }
    }
}
