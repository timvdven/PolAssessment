using System.Text.Json;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services;

public interface IFileHandler
{
    event EventHandler<FileHandler.AnprDataEventArgs> NewAnprDataRead;
}

public class FileHandler : IFileHandler
{
    private readonly ILogger<FileHandler> _logger;
    private readonly IAnprDataReader _anprReader;
    private readonly object _lock = new();
    
    public FileHandler(IHotFolderWatcherData folderWatcher, IAnprDataReader anprReader, ILogger<FileHandler> logger)
    {
        _logger = logger;
        _anprReader = anprReader;
        folderWatcher.Created += FolderWatcher_Created;
    }

    public event EventHandler<AnprDataEventArgs>? NewAnprDataRead;

    private void OnAnprRead(AnprDataEventArgs e)
    {
        _logger.LogInformation("New ANPR data read.");
        NewAnprDataRead?.Invoke(this, e);
    }

    private void FolderWatcher_Created(object sender, FileSystemEventArgs e)
    {
        if (!File.Exists(e.FullPath))
        {
            // Must be a directory
            _logger.LogInformation("[AnprHandler] New directory (?) detected: {FullPath}", e.FullPath);
            return;
        }
        _logger.LogInformation("[AnprHandler] New file detected: {FullPath}", e.FullPath);

        // Somehow I need to read, process and delete the file atomically
        lock (_lock)
        {
            try
            {
                var anprData = _anprReader.ReadAnprData(e.FullPath);
                OnAnprRead(new AnprDataEventArgs(anprData));
            }
            catch (JsonException ex)
            {
                // Log the error and continue
                _logger.LogError(ex, "Error while reading ANPR data: {FullPath}", e.FullPath);
            }
            finally
            {
                // Delete the file
                File.Delete(e.FullPath);
            }
        }
    }

    public class AnprDataEventArgs(AnprData data) : EventArgs
    {
        public AnprData Data { get; } = data;
    }
}
