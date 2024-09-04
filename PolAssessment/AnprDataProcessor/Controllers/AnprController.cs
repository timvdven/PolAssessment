using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Extensions;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprDataProcessor.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnprController(ILogger<AnprController> logger, AnprDataDbContext context) : ControllerBase
{
    private readonly ILogger<AnprController> _logger = logger;
    private readonly AnprDataDbContext _context = context;

    // GET: api/Upload/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AnprRecord>> GetAnprRecord(long id)
    {
        var userId = User.GetUserId();
        var anprRecord = await _context.AnprRecords.FindAsync(id);

        if (anprRecord == null)
        {
            return NotFound();
        }

        return anprRecord;
    }

    // POST: api/Upload
    [HttpPost]
    public async Task<ActionResult<AnprRecord>> PostAnprRecord(AnprRecord anprRecord)
    {
        var userId = User.GetUserId();

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                _context.AnprRecords.Add(anprRecord);
                await _context.SaveChangesAsync();

                _ = _context.AnprRecordUploadUsers.Add(new AnprRecordUploadUser 
                { 
                    AnprRecordId = anprRecord.Id,
                    UploadUserId = userId 
                });
                await _context.SaveChangesAsync();
                
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "An error occurred while saving the record.");
                throw;
            }
        }

        return CreatedAtAction("GetAnprRecord", new { id = anprRecord.Id }, anprRecord);
    }
}
