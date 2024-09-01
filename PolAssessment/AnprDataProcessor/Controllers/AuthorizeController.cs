using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Services;
using PolAssessment.Shared.Services;

namespace PolAssessment.AnprDataProcessor.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthorizeController(ILogger<AuthorizeController> logger, IAnprDataDbContext anprDataDbContext, IAuthTokenHandler authTokenHandler, IHashService hashService) : ControllerBase
{
    private readonly IAnprDataDbContext _anprDataDbContext = anprDataDbContext;
    private readonly IAuthTokenHandler _authTokenHandler = authTokenHandler;
    private readonly ILogger<AuthorizeController> _logger = logger;
    private readonly IHashService _hashService = hashService;

    [HttpGet]
    public async Task<ActionResult<string>> Get(
        [FromHeader(Name = "Client-Id")] string clientId,
        [FromHeader(Name = "Client-Secret")] string clientSecret)
    {
        _logger.LogInformation("Authorize request received.");
        
        var hashedClientSecret = _hashService.GetHash(clientSecret);
        var candidateUser = await _anprDataDbContext.UploadUsers.FirstOrDefaultAsync(x => x.ClientId == clientId && x.HashedClientSecret == hashedClientSecret);
        
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
