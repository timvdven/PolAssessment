using Microsoft.Extensions.Configuration;
using PolAssessment.AnprEnricher.Configuration;
using System.Globalization;

namespace PolAssessment.AnprEnricher.Extensions;

public static class ConfigurationExtensions
{
    public static string GetHotFolderTgzPath(this IConfiguration config)
    {
        var result = config[FieldNames.HotFolderTgzPath];
        return result ?? throw new MissingFieldException(FieldNames.HotFolderTgzPath);
    }

    public static string GetHotFolderDataPath(this IConfiguration config)
    {
        var result = config[FieldNames.HotFolderDataPath];
        return result ?? throw new MissingFieldException(FieldNames.HotFolderDataPath);
    }

    public static int GetMaxRetriesForReadingFile(this IConfiguration config)
    {
        var result = config[FieldNames.MaxRetriesForReadingFile];
        if (int.TryParse(result, out int trials))
        {
            return trials;
        }
        return 0;
    }

    public static int GetTimeOutForRetry(this IConfiguration config)
    {
        var result = config[FieldNames.TimeOutForRetry];
        if (int.TryParse(result, out int timeOut))
        {
            return timeOut;
        }
        return 500;
    }

    public static string GetVehicleEnricherUrl(this IConfiguration config)
    {
        var result = config[FieldNames.VehicleEnricherUrl];
        return result ?? throw new MissingFieldException(FieldNames.VehicleEnricherUrl);
    }

    public static string GetVehicleEnricherUrl(this IConfiguration config, string numberPlate)
    {
        var rawString = GetVehicleEnricherUrl(config);
        return string.Format(rawString, numberPlate);
    }

    public static string GetLocationEnricherUrl(this IConfiguration config)
    {
        var result = config[FieldNames.LocationEnricherUrl];
        return result ?? throw new MissingFieldException(FieldNames.LocationEnricherUrl);
    }

    public static string GetLocationEnricherApiKey(this IConfiguration config)
    {
        var result = config[FieldNames.LocationEnricherApiKey];
        return result ?? throw new MissingFieldException(FieldNames.LocationEnricherApiKey);
    }

    public static string GetLocationEnricherUrl(this IConfiguration config, double latitude, double longitude)
    {
        var apiKey = GetLocationEnricherApiKey(config);

        var rawString = GetLocationEnricherUrl(config);

        return string.Format(CultureInfo.InvariantCulture, rawString, latitude, longitude, apiKey);
    }
}
