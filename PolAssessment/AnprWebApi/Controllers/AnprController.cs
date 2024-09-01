using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnprController(ILogger<AnprController> logger, IAnprDataDbContext context) : ControllerBase
{
    private readonly ILogger<AnprController> _logger = logger;
    private readonly IAnprDataDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnprRecord>>> GetAnprRecords(
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate, 
        [FromQuery] string? plate,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var userId = User.GetUserId();
        var anprRecords = await _context.AnprRecords.Where(x => 
            (!startDate.HasValue || x.ExactDateTime >= startDate.Value) &&
            (!endDate.HasValue || x.ExactDateTime <= endDate.Value) &&
            (string.IsNullOrWhiteSpace(plate) || x.LicensePlate == plate)).ToArrayAsync();

        var myPage = page ?? default;
        var myPageSize = pageSize ?? int.MaxValue;

        return anprRecords?.Skip(myPage * myPageSize)?.Take(myPageSize)?.ToArray() ?? [];
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnprRecord>> GetAnprRecordById(long id)
    {
        var userId = User.GetUserId();
        var anprRecord = await _context.AnprRecords.FindAsync(id);

        if (anprRecord == null)
        {
            return NotFound();
        }

        return anprRecord;
    }
}
