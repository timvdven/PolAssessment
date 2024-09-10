using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PolAssessment.AnprEnricher.App.Configuration;
using PolAssessment.AnprEnricher.App.Services;

namespace AnprEnricher.Tests.Services;

[TestFixture]
class AnprEnrichedDataSenderTests
{
    private readonly Mock<IOptions<AnprDataProcessorConfig>> _config = new();
    private readonly Mock<ILogger<AnprEnrichedDataSender>> _loggerMock = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IEnricherCollection> _enrichers = new();
    private readonly Mock<HttpClient> _httpClient = new();
    private readonly Mock<IDataSendThreadControlService> _dataSendThreadControlService = new();
    private AnprEnrichedDataSender _anprEnrichedDataSender;

    [SetUp]
    public void Setup()
    {
        _anprEnrichedDataSender = new AnprEnrichedDataSender(_loggerMock.Object, _mapper.Object, _enrichers.Object, _httpClient.Object, _tokenService.Object, _config.Object, _dataSendThreadControlService.Object);
    }

    [Test]
    public void SendEnrichedData_ReturnsEnrichedDataOrThrowsException()
    {
        var anprRecord = _anprEnrichedDataSender.MapToAnprRecord(new Dictionary<string, object>
        {
            {"LicensePlate", "ABC123"},
            {"key2", "value2"}
        });
        Assert.Pass();
    }
}