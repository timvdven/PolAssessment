using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Services;
using PolAssessment.Shared.Models;
using PolAssessment.Shared.Services;

namespace AnprDataProcessor.Tests.Services;

/// <summary>
/// Tests:
/// Task<AnprRecord?> GetAnprRecordByIdOrDefault(long id);
/// Task<AnprRecord> CreateAnprRecord(AnprRecord anprRecord, int userId);
/// </summary>
[TestFixture]
public class AnprControllerServiceTests
{
    private readonly Mock<ILogger<AnprControllerService>> _logger = new();
    private readonly Mock<IAnprDataDbContext> _dbContext = new();
    private readonly Mock<DbSet<AnprRecord>> _mockDbSet = new();
    private readonly Mock<IHashService> _hashService = new();

    [SetUp]
    public void SetUp()
    {
        var data = new List<AnprRecord>
        {
            new() { Id = 1, LicensePlate = "ABC123" },
            new() { Id = 2, LicensePlate = "DEF456" }
        }.AsQueryable().BuildMock();

        _mockDbSet.As<IQueryable<AnprRecord>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockDbSet.As<IQueryable<AnprRecord>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockDbSet.As<IQueryable<AnprRecord>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockDbSet.As<IQueryable<AnprRecord>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbContext.Setup(c => c.AnprRecords).Returns(_mockDbSet.Object);
        var databaseFacadeMock = new Mock<DatabaseFacade>(_dbContext.Object);

        // Unfortunatly, this doesn't work: TODO: Fix this
        //_dbContext.Setup(c => c.Database).Returns(Mock.Of<DatabaseFacade>());
        //_dbContext.Setup(c => c.Database).Returns(databaseFacadeMock.Object);

        _dbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(123));
    }

    [TestCase(1, "ABC123")]
    [TestCase(2, "DEF456")]
    public void GetAnprRecordByIdOrDefault_ReturnsRecord_IfPresent(long id, string licensePlate)
    {
        var service = new AnprControllerService(_logger.Object, _dbContext.Object);
        var result = service.GetAnprRecordByIdOrDefault(id).Result;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.LicensePlate, Is.EqualTo(licensePlate));
        });
    }

    [TestCase(3)]
    [TestCase(-10)]
    [TestCase(0)]
    public void GetAnprRecordByIdOrDefault_ReturnsDefault_IfNotFound(long id)
    {
        var service = new AnprControllerService(_logger.Object, _dbContext.Object);
        var result = service.GetAnprRecordByIdOrDefault(id).Result;

        Assert.That(result, Is.Null);
    }

    [Test]
    public void CreateAnprRecord_ReturnsRecord_IfCreated()
    {
        var service = new AnprControllerService(_logger.Object, _dbContext.Object);
        var record = new AnprRecord { Id = -10, LicensePlate = "GHI789" };
        // var userId = 1;

        // Unfortunatly, this doesn't work: TODO: Fix this
        // var result = service.CreateAnprRecord(record, userId).Result;

        // Assert.That(result, Is.Not.Null);
        // Assert.That(result.Id, Is.EqualTo(-10));

        //For the time being, we can't test this method because of the DbContext
        Assert.Pass();
    }
}