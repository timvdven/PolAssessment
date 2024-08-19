using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Models;

namespace PolAssessment.AnprDataProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController(AnprDataDbContext context) : ControllerBase
    {
        private readonly AnprDataDbContext _context = context;

        // GET: api/Upload/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnprRecord>> GetAnprRecord(long id)
        {
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
            _context.AnprRecords.Add(anprRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnprRecord", new { id = anprRecord.Id }, anprRecord);
        }
    }
}
