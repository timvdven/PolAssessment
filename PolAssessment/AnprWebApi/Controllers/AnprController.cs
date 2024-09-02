using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Models.Dto;
using PolAssessment.Shared.Models;
using PolAssessment.Shared.Services;

namespace PolAssessment.AnprWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnprController(ILogger<AnprController> logger, IAnprDataDbContext context, IHashService hashService) : ControllerBase
{
    private readonly ILogger<AnprController> _logger = logger;
    private readonly IAnprDataDbContext _context = context;
    private readonly IHashService _hashService = hashService;

    [HttpGet]
    public async Task<ActionResult<AnprResponse>> GetAnprRecords(
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate, 
        [FromQuery] string? plate,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? hash)
    {
        var userId = User.GetUserId();
        var myPage = page ?? default;
        var myPageSize = pageSize ?? int.MaxValue;

        var anprRecords = await _context.AnprRecords.Where(x => 
            (!startDate.HasValue || x.ExactDateTime >= startDate.Value) &&
            (!endDate.HasValue || x.ExactDateTime <= endDate.Value) &&
            (string.IsNullOrWhiteSpace(plate) || x.LicensePlate == plate))
                .Skip(myPage * myPageSize)
                .Take(myPageSize)
                .ToArrayAsync();

        var hashString = _hashService.GetHash(anprRecords);

        var result = new AnprResponse
        {
            Hash = hashString,
            Result = hashString.Equals(hash) ? [] : anprRecords,
            HttpStatusCode = HttpStatusCode.OK,
            Success = true
        };

        return result;
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
