using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PolAssessment.AnprEnricher.App.Configuration;
using PolAssessment.AnprEnricher.App.Services;

namespace AnprEnricher.Tests.Services;

[TestFixture]
public class AnprDataReaderTests
{
    private readonly Mock<IOptions<FileHandlingConfig>> _config = new();
    private readonly Mock<ILogger<AnprDataReader>> _loggerMock = new();

    [Test]
    public void ReadAnprData_ReturnsAnprDataOrThrowsException()
    {
        _config
            .Setup(x => x.Value)
            .Returns(new FileHandlingConfig
            {
                DelayRetry = 500,
                MaxRetriesForReadingFile = 3
            });

        var fileReaderMock = new Mock<IFileService>();
        fileReaderMock
            .SetupSequence(x => x.ReadAllText(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(JsonData[0])
            .Returns(JsonData[1])
            .Returns(JsonData[2]);

        var anprDataReader = new AnprDataReader(_loggerMock.Object, _config.Object, fileReaderMock.Object);

        var result = anprDataReader.ReadAnprData(It.IsAny<string>());
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Plate, Is.EqualTo("AB-12-CD"));
            Assert.That(result.Coordinates.Latitude, Is.EqualTo(52.0246));
            Assert.That(result.Coordinates.Longitude, Is.EqualTo(4.1750));
            Assert.That(result.DateTime, Is.EqualTo(new DateTime(2024, 8, 1, 12, 0, 0)));
        });

        result = anprDataReader.ReadAnprData(It.IsAny<string>());
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Plate, Is.EqualTo("LX-34-OP"));
            Assert.That(result.Coordinates.Latitude, Is.EqualTo(-52.0246));
            Assert.That(result.Coordinates.Longitude, Is.EqualTo(-4.1750));
            Assert.That(result.DateTime, Is.EqualTo(new DateTime(2014, 8, 31, 11, 0, 0)));
        });

        Assert.Throws<JsonException>(() => anprDataReader.ReadAnprData(It.IsAny<string>()));
        Assert.Throws<ArgumentNullException>(() => anprDataReader.ReadAnprData(It.IsAny<string>()));
    }

    private static readonly string[] JsonData =
    [
        @"{
            ""Plate"": ""AB-12-CD"",
            ""Coordinates"": {
                ""Latitude"": 52.0246,
                ""Longitude"": 4.1750
            },
            ""DateTime"": ""2024-08-01T12:00:00""
        }",
        @"{
            ""Plate"": ""LX-34-OP"",
            ""Coordinates"": {
                ""Latitude"": -52.0246,
                ""Longitude"": -4.1750
            },
            ""DateTime"": ""2014-08-31T11:00:00""
        }",
        "Invalid JSON"
    ];
}
