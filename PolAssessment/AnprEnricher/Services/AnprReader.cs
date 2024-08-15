using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions;
using PolAssessment.AnprEnricher.Models;
using System.Text.Json;

namespace PolAssessment.AnprEnricher.Services;

public interface IAnprReader
{
    AnprData ReadAnprData(string fullPath);
}

public class AnprReader(ILogger<AnprReader> logger, IConfiguration configuration) : IAnprReader
{
    private readonly ILogger<AnprReader> _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    public AnprData ReadAnprData(string fullPath)
    {
        try
        {
            var rawString = ReadAllText(fullPath, _configuration.GetMaxRetriesForReadingFile());
            var result = JsonSerializer.Deserialize<AnprData>(rawString);
            return result ?? throw new JsonException("Could not deserialize the ANPR data.");
        }
        catch(JsonException ex)
        {
            _logger.LogError(ex, "Could not deserialize the ANPR data.");
            throw;
        }
    }

    private string ReadAllText(string fullPath, int trial)
    {
        try
        {
            return File.ReadAllText(fullPath);
        }
        catch (IOException) when (trial > 0)
        {
            _logger.LogWarning("Could not read the file to read json. {trial} attempts left. Retrying...", trial);
            Thread.Sleep(_configuration.GetTimeOutForRetry());
            return ReadAllText(fullPath, trial - 1);
        }
    }
}
