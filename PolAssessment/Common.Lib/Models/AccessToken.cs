namespace PolAssessment.Common.Lib.Models;

public class AccessToken
{
    public required string Token { get; set; }
    public DateTime Expiry { get; set; }
}