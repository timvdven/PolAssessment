using System.Text;

namespace PolAssessment.AnprDataProcessor.Extensions;

public static class ConfigurationManagerExtensions
{
    public static string GetDatabaseConnectionString(this IConfiguration configuration)
    {
        return configuration["Database:ConnectionString"]
            ?? throw new InvalidOperationException("Database is not configured.");
    }

    public static string GetJwtIssuer(this IConfiguration configuration)
    {
        return configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is not configured.");
    }

    public static string GetJwtAudience(this IConfiguration configuration)
    {
        return configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT Audience is not configured.");
    }

    public static byte[] GetJwtKey(this IConfiguration configuration)
    {
        return Encoding.UTF8.GetBytes(configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is not configured."));
    }

    public static double GetJwtExpireMinutes(this IConfiguration configuration)
    {
        return double.Parse(configuration["Jwt:ExpireMinutes"]
            ?? throw new InvalidOperationException("JWT ExpireMinutes is not configured."));
    }
}