using Microsoft.Extensions.Configuration;
using PolAssessment.AnprEnricher.Configuration;

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
}
