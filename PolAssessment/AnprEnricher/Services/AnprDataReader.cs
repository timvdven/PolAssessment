using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;
using PolAssessment.AnprEnricher.Models;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

public interface IAnprDataReader
{
    AnprData ReadAnprData(string fullPath);
}

public class AnprDataReader(ILogger<AnprDataReader> logger, IOptions<FileHandlingConfig> configuration, IFileService fileReader) : IAnprDataReader
{
    private readonly ILogger<AnprDataReader> _logger = logger;
    private readonly FileHandlingConfig _configuration = configuration.Value;
    private readonly IFileService _fileReader = fileReader;

    public AnprData ReadAnprData(string fullPath)
    {
        try
        {
            var rawString = _fileReader.ReadAllText(fullPath, _configuration.MaxRetriesForReadingFile);
            var result = JsonSerializer.Deserialize<AnprData>(rawString);
            return result ?? throw new JsonException("Could not deserialize the ANPR data.");
        }
        catch(JsonException ex)
        {
            if (ex.InnerException?.Message?.StartsWith("'0x00' is an invalid start of a value.") ?? false)
            {
                _logger.LogWarning("'0x00' is an invalid start of a value.");
            }
            _logger.LogError(ex, "Could not deserialize the ANPR data.");
            throw;
        }
    }
}
