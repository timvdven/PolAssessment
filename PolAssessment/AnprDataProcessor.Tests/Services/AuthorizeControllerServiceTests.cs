using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using PolAssessment.AnprDataProcessor.WebApi.DbContexts;
using PolAssessment.AnprDataProcessor.WebApi.Services;
using PolAssessment.Common.Lib.Models;
using PolAssessment.Common.Lib.Models.DataProcessor;
using PolAssessment.Common.Lib.Services;

namespace AnprDataProcessor.Tests.Services;

/// <summary>
/// Tests:
/// Task<AuthorizeResponse> Authorize(AuthorizeRequest request);
/// </summary>
[TestFixture]
public class AuthorizeControllerServiceTests
{
    private readonly Mock<ILogger<AuthorizeControllerService>> _logger = new();
    private readonly Mock<IAnprDataDbContext> _mockContext = new();
    private readonly Mock<DbSet<UploadUser>> _mockDbSet = new();

    [SetUp]
    public void SetUp()
    {
        var data = new List<UploadUser>
        {
            new() { Id = 1, ClientId = "123", HashedClientSecret = "Hashed(456)", UserDescription = "Test User 1" },
            new() { Id = 2, ClientId = "abc", HashedClientSecret = "Hashed(def)", UserDescription = "Test User 2" }
        }.AsQueryable().BuildMock();

        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.UploadUsers).Returns(_mockDbSet.Object);
    }

    private static Mock<IHashService> GetHashServiceMock()
    {
        var hashService = new Mock<IHashService>();
        hashService.Setup(x => x.GetHash(It.IsAny<string>())).Returns((string str) => $"Hashed({str})");
        return hashService;
    }

    private static Mock<IAuthTokenHandler> GetAuthTokenHandlerMock()
    {
        var authTokenHandler = new Mock<IAuthTokenHandler>();
        authTokenHandler
            .Setup(x => x.GenerateToken(It.IsAny<UploadUser>()))
            .Returns((UploadUser user) => new AccessToken { Token = $"Token for {user.UserDescription}" });
        return authTokenHandler;
    }

    [TestCase("123", "456", "Token for Test User 1")]
    [TestCase("abc", "def", "Token for Test User 2")]
    public void Post_ReturnsValidToken(string clientId, string clientSecret, string expectedToken)
    {
        var hashService = GetHashServiceMock();
        var authTokenHandler = GetAuthTokenHandlerMock();

        var authorizeController = new AuthorizeControllerService(
            _logger.Object,
            _mockContext.Object,
            authTokenHandler.Object,
            hashService.Object
        );

        var authorizeRequest = new AuthorizeRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var result = authorizeController.Authorize(authorizeRequest).Result;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.HttpResponseCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result!.AccessToken, Is.Not.Null);
            Assert.That(result!.AccessToken!.Token, Is.EqualTo(expectedToken));

            // Consider moving these to a separate method
            hashService.Verify(x => x.GetHash(clientSecret), Times.Once);
            authTokenHandler.Verify(x => x.GenerateToken(It.IsAny<UploadUser>()), Times.Once);
        });
    }

    [TestCase("invalid", "456")]
    [TestCase("abc", "invalid")]
    public void Post_ReturnsUnauthorized(string clientId, string clientSecret)
    {
        var hashService = GetHashServiceMock();
        var authTokenHandler = GetAuthTokenHandlerMock();

        var authorizeController = new AuthorizeControllerService(
            _logger.Object,
            _mockContext.Object,
            authTokenHandler.Object,
            hashService.Object
        );

        var authorizeRequest = new AuthorizeRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var result = authorizeController.Authorize(authorizeRequest).Result;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.HttpResponseCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(result!.AccessToken, Is.Null);

            // Consider moving these to a separate method
            hashService.Verify(x => x.GetHash(clientSecret), Times.Once);
            authTokenHandler.Verify(x => x.GenerateToken(It.IsAny<UploadUser>()), Times.Never);
        });
    }
}
