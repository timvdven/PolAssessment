using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Services;

namespace PolAssessment.AnprDataProcessor.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthorizeController(ILogger<AuthorizeController> logger, IAnprDataDbContext anprDataDbContext, IAuthTokenHandler authTokenHandler) : ControllerBase
{
    private readonly IAnprDataDbContext _anprDataDbContext = anprDataDbContext;
    private readonly IAuthTokenHandler _authTokenHandler = authTokenHandler;
    private readonly ILogger<AuthorizeController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<string>> Get(
        [FromHeader(Name = "Client-Id")] string clientId,
        [FromHeader(Name = "Client-Secret")] string clientSecret)
    {
        _logger.LogInformation("Authorize request received.");
        
        var candidateUser = await _anprDataDbContext.UploadUsers.FirstOrDefaultAsync(x => x.ClientId == clientId && x.ClientSecret == clientSecret);
        
        if (candidateUser == null)
        {
            _logger.LogWarning("Unauthorized request received.");
            return Unauthorized();
        }
    
        var token = _authTokenHandler.GenerateToken(candidateUser);
        _logger.LogInformation("Token generated successfully.");
    
        return Ok(new { token });
    }
}
