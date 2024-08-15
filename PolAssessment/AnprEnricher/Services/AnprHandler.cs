//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services;

public interface IAnprHandler
{
    event EventHandler<AnprHandler.AnprEventArgs> NewAnprDataRead;
}

public class AnprHandler : IAnprHandler
{
    private readonly ILogger<AnprHandler> _logger;
    private readonly IAnprReader _anprReader;

    public AnprHandler(IHotFolderWatcherData folderWatcher, IAnprReader anprReader, ILogger<AnprHandler> logger)
    {
        _logger = logger;
        _anprReader = anprReader;
        folderWatcher.Created += FolderWatcher_Created;
    }

    public event EventHandler<AnprEventArgs>? NewAnprDataRead;

    protected virtual void OnAnprRead(AnprEventArgs e)
    {
        _logger.LogInformation("New ANPR data read.");
        NewAnprDataRead?.Invoke(this, e);
    }

    private void FolderWatcher_Created(object sender, FileSystemEventArgs e)
    {
        var anprData = _anprReader.ReadAnprData(e.FullPath);
        OnAnprRead(new AnprEventArgs(anprData));
    }

    public class AnprEventArgs(AnprData data) : EventArgs
    {
        public AnprData Data { get; } = data;
    }
}
