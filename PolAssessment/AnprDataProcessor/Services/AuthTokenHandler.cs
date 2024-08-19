using Microsoft.IdentityModel.Tokens;
using PolAssessment.AnprDataProcessor.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PolAssessment.AnprDataProcessor.Services;

public interface IAuthTokenHandler
{
    string GenerateToken(UploadUser uploadUser);
}

public class AuthTokenHandler(IConfiguration configuration) : JwtSecurityTokenHandler, IAuthTokenHandler
{
    private readonly IConfiguration _configuration = configuration;

    private string GetKeyString() => _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
    private byte[] GetKey() => Encoding.UTF8.GetBytes(GetKeyString());
    private SymmetricSecurityKey GetSymmetricSecurityKey() => new(GetKey());
    private SigningCredentials GetSigningCredentials() => new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature);

    private string GetExpireMinutesString() => _configuration["Jwt:ExpireMinutes"] ?? throw new InvalidOperationException("JWT ExpireMinutes is not configured.");
    private double GetExpireMinutes() => double.Parse(GetExpireMinutesString());

    public string GenerateToken(UploadUser uploadUser)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, uploadUser.ClientId),
                new(ClaimTypes.NameIdentifier, uploadUser.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(GetExpireMinutes()),
            SigningCredentials = GetSigningCredentials()
        };
        var token = CreateToken(tokenDescriptor);

        return WriteToken(token);
    }
}
