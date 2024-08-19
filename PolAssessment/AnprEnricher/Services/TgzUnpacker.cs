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
        _logger.LogInformation("[TgzUnpacker] New file detected: {FullPath}", e.FullPath);
        try
        {
            _logger.LogInformation("Handling new file: {FullPath}", e.FullPath);
            HandleNewFile(e.FullPath, _configuration.GetMaxRetriesForReadingFile());
        } 
        catch (Exception ex)
        {
            _logger.LogInformation("Error while handling new file: {FullPath}", ex.InnerException);
            _logger.LogError(ex, "Error: {x}", ex.InnerException);
        }
    }

    private async void HandleNewFile(string fullPath, int trial)
    {
        try
        {
            _logger.LogInformation("Opening file: {FullPath}, trial: {trial}", fullPath, trial);
            using var stream = File.OpenRead(fullPath);

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var amountOfEntries = await Unpack(memoryStream);

            if (amountOfEntries == 0)
            {
                throw new EndOfStreamException();
            }
        }
        catch(IOException) when (trial > 0)
        {
            _logger.LogWarning("Could not read the file to unpack. {trial} attempts left. Retrying...", trial);
            Thread.Sleep(_configuration.GetTimeOutForRetry());
            HandleNewFile(fullPath, trial - 1);
        }
        catch(EndOfStreamException) when (trial > 0)
        {
            _logger.LogWarning("No entries found in the file. {trial} attempts left. Retrying...", trial);
            Thread.Sleep(_configuration.GetTimeOutForRetry());
            HandleNewFile(fullPath, trial - 1);
        }
        catch(EndOfStreamException ex) when (trial == 0)
        {
            _logger.LogError(ex, "No entries found in the file. No more attempts left.");
        }
        catch(InvalidDataException ex)
        {
            _logger.LogError("InvalidDataException: {stacktrace}", ex.InnerException);
        }
    }

    public async Task<int> Unpack(Stream tgzStream)
    {
        using var gzip = new GZipStream(tgzStream, CompressionMode.Decompress);

        using var unzippedStream = new MemoryStream();
        await gzip.CopyToAsync(unzippedStream);
        unzippedStream.Seek(0, SeekOrigin.Begin);

        using var reader = new TarReader(unzippedStream);

        int amountOfEntries = 0;
        _logger.LogInformation("Reading entries...");
        while (await reader.GetNextEntryAsync(copyData: true) is TarEntry entry)
        {
            amountOfEntries++;
            HandleEntry(entry);
        }
        _logger.LogInformation("Finished reading entries with {entries} entries.", amountOfEntries);

        return amountOfEntries;
    }

    private void HandleEntry(TarEntry entry)
    {
        var path = _configuration.GetHotFolderDataPath();

        if (entry.EntryType == TarEntryType.Directory)
        {
            _logger.LogInformation("Entry is a directory: {Name}", entry.Name);
            if (entry.Name.Trim(['.', '/', '\\']).Length == 0)
            {
                _logger.LogInformation("Entry is root directory: ignore");
                // Must be root directory: ignore
                return;
            }

            _logger.LogInformation("Creating directory: {Name}", entry.Name);
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
