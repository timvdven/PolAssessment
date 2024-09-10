using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprDataProcessor.WebApi.DbContexts;
using PolAssessment.Common.Lib.Models;

namespace PolAssessment.AnprDataProcessor.WebApi.Services;

public interface IAnprControllerService
{
    Task<AnprRecord?> GetAnprRecordByIdOrDefault(long id);
    Task<AnprRecord> CreateAnprRecord(AnprRecord anprRecord, int userId);
}

public class AnprControllerService(ILogger<AnprControllerService> logger, IAnprDataDbContext context) : IAnprControllerService
{
    private readonly ILogger<AnprControllerService> _logger = logger;
    private readonly IAnprDataDbContext _context = context;

    public async Task<AnprRecord> CreateAnprRecord(AnprRecord anprRecord, int userId)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var result = _context.AnprRecords.Add(anprRecord);
            await _context.SaveChangesAsync();

            _ = _context.AnprRecordUploadUsers.Add(new AnprRecordUploadUser
            {
                AnprRecordId = anprRecord.Id,
                UploadUserId = userId
            });
            await _context.SaveChangesAsync();

            transaction.Commit();

            return result.Entity;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "An error occurred while saving the record.");
            throw;
        }
    }

    public async Task<AnprRecord?> GetAnprRecordByIdOrDefault(long id)
    {
        var anprRecord = await _context.AnprRecords.FirstOrDefaultAsync(x => x.Id == id);
        return anprRecord;
    }
}