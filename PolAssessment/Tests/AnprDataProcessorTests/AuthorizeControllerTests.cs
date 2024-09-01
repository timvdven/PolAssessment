using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using PolAssessment.AnprDataProcessor.Controllers;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Models;
using PolAssessment.AnprDataProcessor.Services;

namespace AnprDataProcessorTests;

[TestFixture]
public class AuthorizeControllerTests
{
    private readonly Mock<ILogger<AuthorizeController>> _logger = new();
    private readonly Mock<IAnprDataDbContext> _mockContext = new();
    private readonly Mock<DbSet<UploadUser>> _mockDbSet = new();

    [SetUp]
    public void SetUp()
    {
        var data = new List<UploadUser>
        {
            new() { Id = 1, ClientId = "123", HashedClientSecret = "456", UserDescription = "Test User 1" },
            new() { Id = 2, ClientId = "abc", HashedClientSecret = "def", UserDescription = "Test User 2" }
        }.AsQueryable().BuildMock();

        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockDbSet.As<IQueryable<UploadUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.UploadUsers).Returns(_mockDbSet.Object);
    }

    [TestCase("123", "456", "Token for Test User 1")]
    [TestCase("abc", "def", "Token for Test User 2")]
    public void Get_ReturnsValidToken(string clientId, string clientSecret, string expectedToken)
    {       
        var authTokenHandlerMock = new Mock<IAuthTokenHandler>();
        authTokenHandlerMock
            .Setup(x => x.GenerateToken(It.IsAny<UploadUser>()))
            .Returns((UploadUser user) => $"Token for {user.UserDescription}");

        var authorizeController = new AuthorizeController(
            _logger.Object,
            _mockContext.Object,
            authTokenHandlerMock.Object
        );

        var actionResult = authorizeController.Get(clientId, clientSecret).Result;
        var result = actionResult.Result as OkObjectResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(200, result!.StatusCode);
        ClassicAssert.NotNull(result!.Value);

        var resultValue = result!.Value!;
        var value = resultValue!.GetType().GetProperty("token")!.GetValue(resultValue);
        ClassicAssert.AreEqual(expectedToken, value);
    }

    [TestCase("invalid", "456")]
    [TestCase("abc", "invalid")]
    public void Get_ReturnsUnauthorized(string clientId, string clientSecret)
    {       
        var authTokenHandlerMock = new Mock<IAuthTokenHandler>();
        authTokenHandlerMock
            .Setup(x => x.GenerateToken(It.IsAny<UploadUser>()))
            .Returns("Whatever");

        var authorizeController = new AuthorizeController(
            _logger.Object,
            _mockContext.Object,
            authTokenHandlerMock.Object
        );

        var actionResult = authorizeController.Get(clientId, clientSecret).Result;
        var result = actionResult.Result as UnauthorizedResult;
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(401, result!.StatusCode);
    }
}
