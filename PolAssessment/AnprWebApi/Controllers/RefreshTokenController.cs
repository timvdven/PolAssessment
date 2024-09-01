using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Models.Dto;
using PolAssessment.AnprWebApi.Services;

namespace PolAssessment.AnprWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RefreshTokenController(IWebApiDbContext webApiDbContext, IAuthTokenHandler authTokenHandler) : ControllerBase
{
    private readonly IWebApiDbContext _webApiDbContext = webApiDbContext;
    private readonly IAuthTokenHandler _authTokenHandler = authTokenHandler;

    [HttpGet]
    public async Task<ActionResult<LoginResponse>> Get()
    {
        var userId = User.GetUserId();
        var user = await _webApiDbContext.WebUsers.FindAsync(userId);

        var expiration = User.Expiration();

        var result = new LoginResponse
        {
            HttpStatusCode = HttpStatusCode.OK,
            Success = true,
            Token = _authTokenHandler.GenerateToken(user!)
        };
        return Ok(result);
    }
}
