using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprDataProcessor.WebApi.Services;
using PolAssessment.Common.Lib.Models.DataProcessor;

namespace PolAssessment.AnprDataProcessor.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthorizeController(ILogger<AuthorizeController> logger, IAuthorizeControllerService authorizeControllerService) : ControllerBase
{
    private readonly ILogger<AuthorizeController> _logger = logger;
    private readonly IAuthorizeControllerService _authorizeControllerService = authorizeControllerService;

    [HttpGet]
    public async Task<ActionResult<AuthorizeResponse>> Get(
        [FromHeader(Name = "Client-Id")] string clientId,
        [FromHeader(Name = "Client-Secret")] string clientSecret)
    {
        _logger.LogInformation("Authorize request received from get.");

        var result = await _authorizeControllerService.Authorize(new AuthorizeRequest
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        });

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<AuthorizeResponse>> Post(AuthorizeRequest request)
    {
        _logger.LogInformation("Authorize request received from post.");

        var result = await _authorizeControllerService.Authorize(request);
        return result;
    }
}
