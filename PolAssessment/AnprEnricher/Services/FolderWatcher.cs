using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Configuration;

namespace PolAssessment.AnprEnricher.Services;

public interface IHotFolderWatcherTgz
{
    event FileSystemEventHandler Created;
}

public interface IHotFolderWatcherData
{
    event FileSystemEventHandler Created;
}

public class FolderWatcher(ILogger<FolderWatcher> logger, string path) : FileSystemWatcher(path), IHotFolderWatcherTgz, IHotFolderWatcherData
{
    private readonly ILogger<FolderWatcher> _logger = logger;

    private void SetBaseSettings()
    {
        NotifyFilter = NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName;

        IncludeSubdirectories = true;
        EnableRaisingEvents = true;
        _logger.LogInformation("FolderWatcher is watching: {path}", Path);
    }

    #region Factory
    public static FolderWatcher CreateHotFolderWatcher<T>(IServiceProvider provider)
    {
        var logger = provider.GetService<ILogger<FolderWatcher>>() ?? throw new Exception("Can't locate logger");
        var configuration = provider.GetService<IConfiguration>() ?? throw new Exception("Can't locate configuration");
        var hotFolderConfig = configuration.GetSection("HotFolder").Get<HotFolderConfig>()!;

        var path = GetFullPath(logger, GetCorrectPath<T>(hotFolderConfig), typeof(T).Name);
        var candidate = new FolderWatcher(logger, path);
        candidate.SetBaseSettings();

        return candidate;
    }

    private static string GetFullPath(ILogger<FolderWatcher> logger, string path, string name)
    {
        var candidate = new DirectoryInfo(path);
        if (!candidate.Exists)
        {
            var dirInfo = Directory.CreateDirectory(path);
            logger.LogInformation("Created directory: {path}", dirInfo.FullName);  
        }

        logger.LogInformation("{name} is watching folder: {path}", name, candidate.FullName);
        return candidate.FullName;
    }

    private static string GetCorrectPath<T>(HotFolderConfig configuration)
    {
        if (typeof(T) == typeof(IHotFolderWatcherTgz))
        {
            return configuration.TgzPath;
        }

        if (typeof(T) == typeof(IHotFolderWatcherData))
        {
            return configuration.DataPath;
        }

        throw new ArgumentException("Unknown type");
    }
    #endregion
}
