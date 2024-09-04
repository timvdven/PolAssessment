using Microsoft.IdentityModel.Tokens;
using PolAssessment.AnprDataProcessor.Extensions;
using PolAssessment.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PolAssessment.AnprDataProcessor.Services;

public interface IAuthTokenHandler
{
    string GenerateToken(UploadUser uploadUser);
}

public class AuthTokenHandler(IConfiguration configuration) : JwtSecurityTokenHandler, IAuthTokenHandler
{
    private readonly IConfiguration _configuration = configuration;

    private SymmetricSecurityKey GetSymmetricSecurityKey() => new(_configuration.GetJwtKey());
    private SigningCredentials GetSigningCredentials() => new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

    public string GenerateToken(UploadUser uploadUser)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration.GetJwtAudience(),
            Issuer = _configuration.GetJwtIssuer(),
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Name, uploadUser.ClientId),
                new(ClaimTypes.NameIdentifier, uploadUser.Id.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetJwtExpireMinutes()),
            SigningCredentials = GetSigningCredentials()
        };
        var token = CreateToken(tokenDescriptor);

        return WriteToken(token);
    }
}
