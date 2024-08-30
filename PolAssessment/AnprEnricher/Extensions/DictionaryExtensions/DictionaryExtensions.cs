namespace PolAssessment.AnprEnricher.Extensions.DictionaryExtensions;

public static class DictionaryExtensions
{
    public static string GetValue(this Dictionary<string, object> dictionary, params string[] key)
    {
        return dictionary.GetValue<string>(key);
    }

    public static string? GetValueOrDefault(this Dictionary<string, object> dictionary, params string[] key)
    {
        return dictionary.GetValueOrDefault<string>(key);
    }

    public static T GetValue<T>(this Dictionary<string, object> dictionary, params string[] key)
    {
        var candidate = dictionary.GetValueOrDefault<T>(key);

        return candidate ?? throw new Exception($"Key '{key}' not found in dictionary");
    }

    public static T? GetValueOrDefault<T>(this Dictionary<string, object> dictionary, params string[] key)
    {
        while(key.Length > 1)
        {
            var scope = dictionary.GetValueOrDefault<Dictionary<string, object>>(key[0]);
            if (scope == default)
            {
                // Scope not found
                return default;
            }
            
            return GetValueOrDefault<T>(scope, key.Skip(1).ToArray());
        }

        if (dictionary.TryGetValue(key[0], out object? value))
        {
            return (T)value;
        }

        return default;
    }
}
