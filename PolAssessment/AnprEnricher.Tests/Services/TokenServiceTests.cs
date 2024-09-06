using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PolAssessment.AnprEnricher.Configuration;
using PolAssessment.AnprEnricher.Services;

namespace AnprEnricher.Tests.Services;

public class TokenServiceTests
{
    private readonly Mock<IOptions<AnprDataProcessorConfig>> _config = new();
    private readonly Mock<HttpClient> _httpClientMock = new();
    private readonly Mock<ILogger<TokenService>> _loggerMock = new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void DeserializeAuthorizeResponse_ReturnsValidToken()
    {
        var responseContent = "{\"accessToken\":{\"token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjNkNmYwZDk4ZjAwYjIwNGU5ODAwOTk4ZWNmODQyN2UiLCJuYW1laWQiOiIxIiwibmJmIjoxNzI1NjMxMTc5LCJleHAiOjE3MjU2MzQ3NzksImlhdCI6MTcyNTYzMTE3OSwiaXNzIjoiYW5wci1kYXRhLXByb2Nlc3NvciIsImF1ZCI6ImFucHItZW5yaWNoZXIifQ.mPrL0qJgzIFMjv1LvErLWmYd0iQlGt4LquEsKoVYBoI\",\"expiry\":\"2024-09-06T14:59:39.614274Z\"},\"success\":true,\"httpResponseCode\":200,\"errorMessage\":null}";
        var tokenService = new TokenService(_httpClientMock.Object, _loggerMock.Object, _config.Object, new JsonSerializerOptionsConfig());
        var result = tokenService.DeserializeAuthorizeResponse(responseContent);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccessToken, Is.Not.Null);
        Assert.That(result.AccessToken.Token, Is.Not.Null);
        Assert.That(result.AccessToken.Token, Is.GreaterThan("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"));
    }
}
