using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolAssessment.AnprEnricher.Configuration;

namespace PolAssessment.AnprEnricher.Services;

public interface IFileService
{
    string ReadAllText(string path, int trial);
    bool Exists(string path);
    void Delete(string path);
}

public class FileService(ILogger<FileService> logger, IOptions<FileHandlingConfig> configuration) : IFileService
{
    private readonly ILogger<FileService> _logger = logger;
    private readonly FileHandlingConfig _configuration = configuration.Value;

    public void Delete(string path)
    {
        File.Delete(path);
    }

    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public string ReadAllText(string fullPath, int trial)
    {
        try
        {
            return File.ReadAllText(fullPath);
        }
        catch (IOException) when (trial > 0)
        {
            _logger.LogWarning("Could not read the file to read json. {trial} attempts left. Retrying...", trial);
            Thread.Sleep(millisecondsTimeout: _configuration.DelayRetry);
            return ReadAllText(fullPath, trial - 1);
        }
    }
}
