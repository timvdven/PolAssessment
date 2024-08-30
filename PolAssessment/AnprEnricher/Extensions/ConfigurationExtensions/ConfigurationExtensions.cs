using Microsoft.Extensions.Configuration;
using PolAssessment.AnprEnricher.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace PolAssessment.AnprEnricher.Extensions.ConfigurationExtensions;

public static class ConfigurationExtensions
{
    public static string GetHotFolderTgzPath(this IConfiguration config)
    {
        var result = config[FieldNames.HotFolder.TgzPath];
        return result ?? throw new MissingFieldException(FieldNames.HotFolder.TgzPath);
    }

    public static string GetHotFolderDataPath(this IConfiguration config)
    {
        var result = config[FieldNames.HotFolder.DataPath];
        return result ?? throw new MissingFieldException(FieldNames.HotFolder.DataPath);
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
        var result = string.Concat(config[FieldNames.VehicleEnricher.BaseUrl], config[FieldNames.VehicleEnricher.Query]);
        return result ?? throw new MissingFieldException(FieldNames.VehicleEnricher.BaseUrl);
    }

    public static string GetVehicleEnricherUrl(this IConfiguration config, string numberPlate)
    {
        var rawString = GetVehicleEnricherUrl(config);
        return string.Format(rawString, numberPlate);
    }

    public static string GetLocationEnricherUrl(this IConfiguration config)
    {
        var result = config[FieldNames.LocationEnricher.BaseUrl];
        return result ?? throw new MissingFieldException(FieldNames.LocationEnricher.BaseUrl);
    }

    public static string GetLocationEnricherApiKey(this IConfiguration config)
    {
        var result = config[FieldNames.LocationEnricher.ApiKey];
        return result ?? throw new MissingFieldException(FieldNames.LocationEnricher.ApiKey);
    }

    public static string GetLocationEnricherUrl(this IConfiguration config, double latitude, double longitude)
    {
        var apiKey = GetLocationEnricherApiKey(config);
        var rawString = GetLocationEnricherUrl(config);

        return string.Format(CultureInfo.InvariantCulture, rawString, latitude, longitude, apiKey);
    }

    public static string GetAnprDataProcessorAuthorizeUrl(this IConfiguration config)
    {
        var result = string.Concat(config[FieldNames.AnprDataProcessor.BaseUrl], config[FieldNames.AnprDataProcessor.Operation.Authorize]);
        return result ?? throw new MissingFieldException(GetMethodName());
    }

    public static string GetAnprDataProcessorAnprUrl(this IConfiguration config)
    {
        var result = string.Concat(config[FieldNames.AnprDataProcessor.BaseUrl], config[FieldNames.AnprDataProcessor.Operation.Anpr]);
        return result ?? throw new MissingFieldException(GetMethodName());
    }

    public static string GetAnprDataProcessorClientId(this IConfiguration config)
    {
        var result = config[FieldNames.AnprDataProcessor.ClientId];
        return result ?? throw new MissingFieldException(FieldNames.AnprDataProcessor.ClientId);
    }

    public static string GetAnprDataProcessorClientSecret(this IConfiguration config)
    {
        var result = config[FieldNames.AnprDataProcessor.ClientSecret];
        return result ?? throw new MissingFieldException(FieldNames.AnprDataProcessor.ClientSecret);
    }

    private static string GetMethodName([CallerMemberName] string? methodName = null)
    {
        return methodName ?? "UNKNOWN";
    }
}
