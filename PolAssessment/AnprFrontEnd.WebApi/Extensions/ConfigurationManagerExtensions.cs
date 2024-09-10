using System.Text;

namespace PolAssessment.AnprWebApi.Extensions;

public static class ConfigurationManagerExtensions
{
    public static string GetAnprDatabaseConnectionString(this IConfiguration configuration)
    {
        return configuration["Database:ConnectionStringAnpr"]
            ?? throw new InvalidOperationException("ANPR database is not configured.");
    }

        public static string GetWebApiDatabaseConnectionString(this IConfiguration configuration)
    {
        return configuration["Database:ConnectionStringWebApi"]
            ?? throw new InvalidOperationException("WebApi database is not configured.");
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