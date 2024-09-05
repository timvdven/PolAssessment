using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Configuration;
using PolAssessment.AnprEnricher.Services;
using PolAssessment.AnprEnricher.Services.Enrichers;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettingsSecrets.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(logging => logging.AddConsole())

    .Configure<HotFolderConfig>(configuration.GetSection("HotFolder"))
    .AddSingleton<IHotFolderWatcherTgz>(FolderWatcher.CreateHotFolderWatcher<IHotFolderWatcherTgz>)
    .AddSingleton<IHotFolderWatcherData>(FolderWatcher.CreateHotFolderWatcher<IHotFolderWatcherData>)

    .Configure<FileHandlingConfig>(configuration.GetSection("FileHandling"))
    .AddScoped<TgzUnpacker>()
    .AddScoped<IFileService, FileService>()
    .AddScoped<IFileHandler, FileHandler>()

    .AddScoped<IAnprDataReader, AnprDataReader>()
    .AddScoped<IEnricherCollection, EnricherCollectionHandler>()

    .Configure<LocationEnricherConfig>(configuration.GetSection("LocationEnricher"))
    .AddScoped<ILocationDataUrlService, LocationDataUrlService>()
    .AddScoped<IEnricher, LocationDataEnricher>()

    .Configure<VehicleEnricherConfig>(configuration.GetSection("VehicleEnricher"))
    .AddScoped<IVehicleDataUrlService, VehicleDataUrlService>()
    .AddScoped<IEnricher, VehicleDataEnricher>()

    .AddSingleton<AnprEnrichedDataSender>()

    .Configure<AnprDataProcessorConfig>(configuration.GetSection("AnprDataProcessor"))
    .AddSingleton<ITokenService, TokenService>()

    .AddHttpClient()
    .AddAutoMapper(typeof(Program))
    .BuildServiceProvider();

serviceProvider.GetRequiredService<TgzUnpacker>();
serviceProvider.GetRequiredService<AnprEnrichedDataSender>();

Console.WriteLine("Press 'q' to quit.");
while (Console.Read() != 'q') ;
