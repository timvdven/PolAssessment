using Microsoft.IdentityModel.Tokens;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PolAssessment.AnprWebApi.Services;

public interface IAuthTokenHandler
{
    string GenerateToken(WebUser webUser);
}

public class AuthTokenHandler(IConfiguration configuration) : JwtSecurityTokenHandler, IAuthTokenHandler
{
    private readonly IConfiguration _configuration = configuration;

    private SymmetricSecurityKey GetSymmetricSecurityKey() => new(_configuration.GetJwtKey());
    private SigningCredentials GetSigningCredentials() => new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

    public string GenerateToken(WebUser webUser)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration.GetJwtAudience(),
            Issuer = _configuration.GetJwtIssuer(),
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Name, webUser.Username),
                new(ClaimTypes.NameIdentifier, webUser.Id.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetJwtExpireMinutes()),
            SigningCredentials = GetSigningCredentials()
        };
        var token = CreateToken(tokenDescriptor);

        return WriteToken(token);
    }
}
