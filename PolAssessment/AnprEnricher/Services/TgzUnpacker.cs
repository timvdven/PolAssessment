using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Extensions;
using System.Formats.Tar;
using System.IO.Compression;

namespace PolAssessment.AnprEnricher.Services;

public class TgzUnpacker
{
    private readonly ILogger<TgzUnpacker> _logger;
    private readonly IConfiguration _configuration;

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
            _logger.LogInformation("Entry name: {Name}, entry type: {EntryType}", entry.Name, entry.EntryType);

            if (entry.Name.Length > 3 )
            {
                entry.ExtractToFile(destinationFileName: Path.Join(_configuration.GetHotFolderDataPath(), entry.Name), overwrite: true);
            }
        }
    }
}
