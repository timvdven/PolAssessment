using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using PolAssessment.AnprDataProcessor.Controllers;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Services;
using PolAssessment.Shared.Models;
using PolAssessment.Shared.Models.DataProcessor;
using PolAssessment.Shared.Services;

namespace AnprDataProcessorTests;

[TestFixture]
public class AuthorizeControllerTests
{
    private readonly Mock<ILogger<AuthorizeController>> _logger = new();
    private readonly Mock<IAnprDataDbContext> _mockContext = new();
    private readonly Mock<DbSet<UploadUser>> _mockDbSet = new();
    private readonly Mock<IHashService> _hashService = new();

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

        _hashService.Setup(x => x.GetHash(It.IsAny<string>())).Returns((string str) => $"Hashed({str})");
    }

    [TestCase("123", "456", "Token for Test User 1")]
    [TestCase("abc", "def", "Token for Test User 2")]
    public void Post_ReturnsValidToken(string clientId, string clientSecret, string expectedToken)
    {       
        var authTokenHandlerMock = new Mock<IAuthTokenHandler>();
        authTokenHandlerMock
            .Setup(x => x.GenerateToken(It.IsAny<UploadUser>()))
            .Returns((UploadUser user) => new AccessToken{ Token = $"Token for {user.UserDescription}" });

        var authorizeController = new AuthorizeController(
            _logger.Object,
            _mockContext.Object,
            authTokenHandlerMock.Object, 
            _hashService.Object
        );

        var authorizeRequest = new AuthorizeRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
        var actionResult = authorizeController.Post(authorizeRequest).Result;
        var actionResultResult = actionResult.Result as OkObjectResult;
        var result = actionResultResult?.Value as AuthorizeResponse;

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result!.HttpResponseCode);
        ClassicAssert.NotNull(result!.AccessToken);
        ClassicAssert.AreEqual(expectedToken, result!.AccessToken!.Token);
    }

    [TestCase("invalid", "456")]
    [TestCase("abc", "invalid")]
    public void Post_ReturnsUnauthorized(string clientId, string clientSecret)
    {       
        var authTokenHandlerMock = new Mock<IAuthTokenHandler>();
        authTokenHandlerMock
            .Setup(x => x.GenerateToken(It.IsAny<UploadUser>()))
            .Returns((UploadUser user) => new AccessToken{ Token = $"Token for {user.UserDescription}" });

        var authorizeController = new AuthorizeController(
            _logger.Object,
            _mockContext.Object,
            authTokenHandlerMock.Object,
            _hashService.Object
        );

        var authorizeRequest = new AuthorizeRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
        var actionResult = authorizeController.Post(authorizeRequest).Result;
        var actionResultResult = actionResult.Result as OkObjectResult;
        var result = actionResultResult?.Value as AuthorizeResponse;
        
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(HttpStatusCode.Unauthorized, result!.HttpResponseCode);
        ClassicAssert.IsNull(result!.AccessToken);
    }
}
