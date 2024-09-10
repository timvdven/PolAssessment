using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolAssessment.Common.Lib.Configuration;
using PolAssessment.Common.Lib.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PolAssessment.AnprDataProcessor.WebApi.Services;

public interface IAuthTokenHandler
{
    AccessToken GenerateToken(UploadUser user);
}

public class AuthTokenHandler(IOptions<JwtConfig> configuration) : JwtSecurityTokenHandler, IAuthTokenHandler
{
    private readonly IOptions<JwtConfig> _configuration = configuration;
    private SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(_configuration.Value.Key));
    private SigningCredentials GetSigningCredentials() => new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

    public AccessToken GenerateToken(UploadUser user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration.Value.Audience,
            Issuer = _configuration.Value.Issuer,
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Name, user.ClientId),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.Value.ExpiryInMinutes),
            SigningCredentials = GetSigningCredentials()
        };
        var token = CreateToken(tokenDescriptor);

        return new AccessToken
        {
            Token = WriteToken(token),
            Expiry = tokenDescriptor.Expires.Value
        };
    }
}
