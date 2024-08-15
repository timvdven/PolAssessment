using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PolAssessment.AnprEnricher.Services;
using PolAssessment.AnprEnricher.Services.Enrichers;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(logging => logging.AddConsole())
    .AddSingleton<IConfiguration>(_ => configuration)
    .AddSingleton<IHotFolderWatcherTgz>(FolderWatcher.CreateHotFolderWatcher<IHotFolderWatcherTgz>)
    .AddSingleton<IHotFolderWatcherData>(FolderWatcher.CreateHotFolderWatcher<IHotFolderWatcherData>)
    .AddScoped<TgzUnpacker>()
    .AddScoped<IAnprReader, AnprReader>()
    .AddScoped<IAnprHandler, AnprHandler>()
    .AddScoped<IEnricherCollection, EnricherCollection>()
    .AddScoped<IEnricher, LocationDataEnricher>()
    .AddScoped<IEnricher, VehicleDataEnricher>()
    .AddScoped<AnprEnrichedDataSender>()
    .BuildServiceProvider();

serviceProvider.GetRequiredService<TgzUnpacker>();
serviceProvider.GetRequiredService<AnprEnrichedDataSender>();

Console.WriteLine("Press enter to exit.");
Console.ReadLine();
