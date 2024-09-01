using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Models.Dto;
using PolAssessment.AnprWebApi.Services;
using PolAssessment.Shared.Services;

namespace PolAssessment.AnprWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController(ILogger<LoginController> logger, IWebApiDbContext webapiDataDbContext, IAuthTokenHandler authTokenHandler, IHashService hashService) : ControllerBase
{
    private readonly IWebApiDbContext _webapiDataDbContext = webapiDataDbContext;
    private readonly IAuthTokenHandler _authTokenHandler = authTokenHandler;
    private readonly ILogger<LoginController> _logger = logger;
    private readonly IHashService _hashService = hashService;

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Post([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("Authorize request received.");

        var hashedPassword = _hashService.GetHash(loginRequest.Password);
        var candidateUser = await _webapiDataDbContext.WebUsers.FirstOrDefaultAsync(x => x.Username == loginRequest.Username && x.HashedPassword == hashedPassword);
        var response = new LoginResponse();

        if (candidateUser == null)
        {
            _logger.LogWarning("Unauthorized request received.");
            response.HttpStatusCode = HttpStatusCode.Unauthorized;
            response.Success = false;
            return Ok(response);
        }

        var token = _authTokenHandler.GenerateToken(candidateUser);
        _logger.LogInformation("Token generated successfully.");
        response.HttpStatusCode = HttpStatusCode.OK;
        response.Success = true;
        response.Token = token;
        return Ok(response);
    }
}
