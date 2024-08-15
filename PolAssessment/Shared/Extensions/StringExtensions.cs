using Microsoft.Extensions.Configuration;

namespace PolAssessment.Shared.Extensions;

public static class StringExtensions
{
    #region Environment StringExtensions
    public static string GetEnvironmentValue(this string str, IConfiguration configuration)
    {
        var result = configuration[str];
        ArgumentNullException.ThrowIfNull(result);
        return result;
    }

    public static string GetEnvironmentValue(this string str)
    {
        str.TryGetEnvironmentValue(out var result);
        ArgumentNullException.ThrowIfNull(result);
        return result;
    }

    public static bool TryGetEnvironmentValue(this string str, out string? result)
    {
        result = Environment.GetEnvironmentVariable(str);
        return !string.IsNullOrEmpty(result);
    }

    public static bool GetEnvironmentValueBool(this string str, bool defaultValue = false)
    {
        var stringValue = str.GetEnvironmentValue();
        if (bool.TryParse(stringValue, out var result))
        {
            return result;
        }
        return defaultValue;
    }

    public static int? GetEnvironmentValueInt(this string str)
    {
        var stringValue = str.GetEnvironmentValue();
        if (int.TryParse(stringValue, out var result))
        {
            return result;
        }
        return null;
    }

    public static int GetEnvironmentValueInt(this string str, int defaultValue)
    {
        var stringValue = str.GetEnvironmentValue();
        if (int.TryParse(stringValue, out var result))
        {
            return result;
        }
        return defaultValue;
    }
    #endregion
}
