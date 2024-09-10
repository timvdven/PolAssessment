using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Models.Dto;
using PolAssessment.AnprWebApi.Services;
using PolAssessment.Common.Lib.Models;
using PolAssessment.Common.Lib.Services;

namespace PolAssessment.AnprFrontEnd.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnprController(ILogger<AnprController> logger, IAnprDataDbContext context, IHashService hashService, IAnprQueryService anprQueryService) : ControllerBase
{
    private readonly ILogger<AnprController> _logger = logger;
    private readonly IAnprDataDbContext _context = context;
    private readonly IHashService _hashService = hashService;
    private readonly IAnprQueryService _anprQueryService = anprQueryService;

    [HttpGet]
    public async Task<ActionResult<AnprResponse>> GetAnprRecords([FromQuery] AnprRequest anprRequest)
    {
        if (!string.IsNullOrEmpty(anprRequest.Hash) || !anprRequest.MinimumUploadDate.HasValue)
        {
            // Only log the request if the user is not requesting a hash or a minimum upload date
            // in order to avoid logging the auto refresh requests
            _logger.LogInformation("User {userId} requested ANPR records", User.GetUserId());
        }

        var anprRecords = await _anprQueryService.GetAnprRecords(anprRequest);
        var hashString = _hashService.GetHash(anprRecords);

        var result = new AnprResponse
        {
            Hash = hashString,
            LastFetchDate = DateTime.UtcNow,
            Result = hashString.Equals(anprRequest.Hash) ? [] : anprRecords,
            HttpResponseCode = HttpStatusCode.OK,
            Success = true
        };

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnprRecord>> GetAnprRecordById(long id)
    {
        _logger.LogInformation("User {userId} requested ANPR record {id}", User.GetUserId(), id);

        var anprRecord = await _context.AnprRecords.FindAsync(id);

        if (anprRecord == null)
        {
            return NotFound();
        }

        return anprRecord;
    }
}
