﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprDataProcessor.Extensions;
using PolAssessment.AnprDataProcessor.Services;
using PolAssessment.Shared.Models;

namespace PolAssessment.AnprDataProcessor.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnprController(ILogger<AnprController> logger, IAnprControllerService anprControllerService) : ControllerBase
{
    private readonly ILogger<AnprController> _logger = logger;
    private readonly IAnprControllerService _anprControllerService = anprControllerService;

    [HttpGet("{id}")]
    public async Task<ActionResult<AnprRecord>> Get(long id)
    {
        var anprRecord = await _anprControllerService.GetAnprRecordByIdOrDefault(id);

        if (anprRecord == null)
        {
            return NotFound();
        }

        return anprRecord;
    }

    [HttpPost]
    public async Task<ActionResult<AnprRecord>> Post(AnprRecord anprRecord)
    {
        var userId = User.GetUserId();
        var result = await _anprControllerService.CreateAnprRecord(anprRecord, userId);

        return CreatedAtAction("GetAnprRecord", new { id = result.Id }, result);
    }
}
