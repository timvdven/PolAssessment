namespace PolAssessment.Shared.Configuration;

public class JwtConfig
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpiryInMinutes { get; set; }
}
