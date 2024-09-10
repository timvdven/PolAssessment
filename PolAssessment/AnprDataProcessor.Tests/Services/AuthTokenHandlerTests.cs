using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework.Internal;
using PolAssessment.AnprDataProcessor.WebApi.Services;
using PolAssessment.Common.Lib.Configuration;
using PolAssessment.Common.Lib.Models;

namespace AnprDataProcessor.Tests.Services;

/// <summary>
/// Tests:
/// AccessToken GenerateToken(UploadUser user);
/// </summary>
[TestFixture]
public class AuthTokenHandlerTests
{
    private readonly Mock<IOptions<JwtConfig>> _mockJwtConfig = new();
    private IEnumerable<AccessToken> _accessTokens;
    private IEnumerable<UploadUser> _users;

    [SetUp]
    public void SetUp()
    {
        _mockJwtConfig.Setup(x => x.Value).Returns(new JwtConfig
        {
            Audience = "My Audience Name",
            Issuer = "My Issuer Name",
            Key = "Key_Size_Should_Be_At_Least_32_Characters",
            ExpiryInMinutes = 30
        });

        _users =
        [
            GetUser(1, It.IsAny<string>(), "ClientId_123", It.IsAny<string>()),
            GetUser(2, It.IsAny<string>(), "ClientId_abc", It.IsAny<string>()),
            GetUser(-100, It.IsAny<string>(), "Some Unique Id", It.IsAny<string>()),
        ];

        _accessTokens = _users.Select(user =>
        {
            var authTokenHandler = new AuthTokenHandler(_mockJwtConfig.Object);
            return authTokenHandler.GenerateToken(user);
        });
    }
    
    private static UploadUser GetUser(int id, string userDescription, string clientId, string hashedClientSecret)
    {
        return new UploadUser
        {
            Id = id,
            ClientId = clientId,
            UserDescription = userDescription,
            HashedClientSecret = hashedClientSecret
        };
    }

    [Test]
    public void GenerateToken_CreatesToken_ProperAccessToken()
    {
        foreach (var (user, accessToken) in _users.Zip(_accessTokens, (user, accessToken) => (user, accessToken)))
        {
            Assert.Multiple(() =>
            {
                Assert.That(accessToken, Is.Not.Null);
                Assert.That(accessToken.Token, Is.Not.Null);
                Assert.That(accessToken.Token, Is.Not.Empty);
                Assert.That(accessToken.Expiry, Is.Not.EqualTo(default(DateTime)));
            });
        }
    }

    [Test]
    public void GenerateToken_CreatesToken_CorrectGenericClaims()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        foreach (var (user, accessToken) in _users.Zip(_accessTokens, (user, accessToken) => (user, accessToken)))
        {
            var token = tokenHandler.ReadJwtToken(accessToken.Token);

            Assert.Multiple(() =>
            {
                Assert.That(token.Audiences, Contains.Item(_mockJwtConfig.Object.Value.Audience));
                Assert.That(token.Issuer, Is.EqualTo(_mockJwtConfig.Object.Value.Issuer));
                Assert.That(token.ValidTo, Is.AtLeast(DateTime.UtcNow));
                Assert.That(token.ValidTo, Is.AtMost(DateTime.UtcNow.AddMinutes(_mockJwtConfig.Object.Value.ExpiryInMinutes)));
                Assert.That(token.ValidFrom, Is.AtMost(token.ValidTo));
            });
        }
    }

    [Test]
    public void GenerateToken_CreatesToken_CorrectIdentifyingClaims()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        foreach (var (user, accessToken) in _users.Zip(_accessTokens, (user, accessToken) => (user, accessToken)))
        {
            var token = tokenHandler.ReadJwtToken(accessToken.Token);

            var nameId = token.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
            var uniqie_name = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;

            Assert.Multiple(() =>
            {
                Assert.That(nameId, Is.EqualTo(user.Id.ToString()));
                Assert.That(uniqie_name, Is.EqualTo(user.ClientId));
            });
        }
    }

    [Test]
    public void GenerateToken_CreatesToken_WithValidSignature()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        foreach (var (user, accessToken) in _users.Zip(_accessTokens, (user, accessToken) => (user, accessToken)))
        {
            var token = tokenHandler.ReadJwtToken(accessToken.Token);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mockJwtConfig.Object.Value.Key)),
                ValidateIssuer = true,
                ValidIssuer = _mockJwtConfig.Object.Value.Issuer,
                ValidateAudience = true,
                ValidAudience = _mockJwtConfig.Object.Value.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(accessToken.Token, validationParameters, out SecurityToken validatedToken);

            Assert.Multiple(() =>
            {
                Assert.That(principal, Is.Not.Null);
                Assert.That(validatedToken, Is.Not.Null);

                Assert.That(validatedToken.Id, Is.EqualTo(token.Id));
                Assert.That(validatedToken.Issuer, Is.EqualTo(token.Issuer));
                Assert.That(validatedToken.ValidFrom, Is.EqualTo(token.ValidFrom));
                Assert.That(validatedToken.ValidTo, Is.EqualTo(token.ValidTo));
            });
        }
    }
}
