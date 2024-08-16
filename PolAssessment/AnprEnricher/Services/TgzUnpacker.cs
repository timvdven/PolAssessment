using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions;
using System.Formats.Tar;
using System.IO.Compression;

namespace PolAssessment.AnprEnricher.Services;

public class TgzUnpacker
{
    private readonly ILogger<TgzUnpacker> _logger;
    private readonly IConfiguration _configuration;
    private readonly object _lock = new();

    public TgzUnpacker(IConfiguration configuration, IHotFolderWatcherTgz hotFolderWatcher, ILogger<TgzUnpacker> logger)
    {
        _logger = logger;
        _configuration = configuration;
        hotFolderWatcher.Created += HotFolderWatcher_Created;
    }

    private void HotFolderWatcher_Created(object sender, FileSystemEventArgs e)
    {
        HandleNewFile(e.FullPath, _configuration.GetMaxRetriesForReadingFile());
    }

    private void HandleNewFile(string fullPath, int trial)
    {
        try
        {
            var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None) ?? throw new MissingFieldException(nameof(fullPath));
            Unpack(stream);
        }
        catch(IOException) when (trial > 0)
        {
            _logger.LogWarning("Could not read the file to unpack. {trial} attempts left. Retrying...", trial);
            Thread.Sleep(_configuration.GetTimeOutForRetry());
            HandleNewFile(fullPath, trial - 1);
        }
    }

    public async void Unpack(Stream tgzStream)
    {
        using var gzip = new GZipStream(tgzStream, CompressionMode.Decompress);
        using var unzippedStream = new MemoryStream();

        await gzip.CopyToAsync(unzippedStream);
        unzippedStream.Seek(0, SeekOrigin.Begin);

        using var reader = new TarReader(unzippedStream);

        while (reader.GetNextEntry() is TarEntry entry)
        {
            HandleEntry(entry);
        }
    }

    private void HandleEntry(TarEntry entry)
    {
        var path = _configuration.GetHotFolderDataPath();

        if (entry.EntryType == TarEntryType.Directory)
        {
            if (entry.Name.Trim(['.', '/', '\\']).Length == 0)
            {
                // Must be root directory: ignore
                return;
            }

            Directory.CreateDirectory(Path.Combine(path, entry.Name));
            return;
        }

        lock (_lock)
        {
            _logger.LogInformation("Entry name: {Name}, entry type: {EntryType}", entry.Name, entry.EntryType);
            var fullPath = GetUniqueFullPath(path, entry.Name);
            entry.ExtractToFile(destinationFileName: fullPath, overwrite: true);
        }
    }

    private static string GetUniqueFullPath(string path, string filename)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));
        ArgumentNullException.ThrowIfNull(filename, nameof(filename));

        var uniquePath = Path.Combine(path, filename);
        var counter = 1;

        while (File.Exists(uniquePath))
        {
            uniquePath = Path.Combine(path, $"{Path.GetFileNameWithoutExtension(filename)}_{counter}{Path.GetExtension(filename)}");
            counter++;
        }

        return uniquePath;
    }
}
