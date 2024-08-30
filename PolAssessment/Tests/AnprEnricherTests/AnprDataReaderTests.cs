using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using PolAssessment.AnprEnricher.Services;

namespace AnprEnricherTests;

[TestFixture]
public class AnprDataReaderTests
{
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<ILogger<AnprDataReader>> _loggerMock = new();

    [Test]
    public void ReadAnprData_ReturnsAnprDataOrThrowsException()
    {
        var fileReaderMock = new Mock<IFileService>();
        fileReaderMock
            .SetupSequence(x => x.ReadAllText(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(JsonData[0])
            .Returns(JsonData[1])
            .Returns(JsonData[2]);
        
        var anprDataReader = new AnprDataReader(_loggerMock.Object, _configMock.Object, fileReaderMock.Object);
        
        var result = anprDataReader.ReadAnprData(It.IsAny<string>());
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("AB-12-CD", result.Plate);
        ClassicAssert.AreEqual(52.0246, result.Coordinates.Latitude);
        ClassicAssert.AreEqual(4.1750, result.Coordinates.Longitude);
        ClassicAssert.AreEqual(new DateTime(2024, 8, 1, 12, 0, 0), result.DateTime);

        result = anprDataReader.ReadAnprData(It.IsAny<string>());
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("LX-34-OP", result.Plate);
        ClassicAssert.AreEqual(-52.0246, result.Coordinates.Latitude);
        ClassicAssert.AreEqual(-4.1750, result.Coordinates.Longitude);
        ClassicAssert.AreEqual(new DateTime(2014, 8, 31, 11, 0, 0), result.DateTime);

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
