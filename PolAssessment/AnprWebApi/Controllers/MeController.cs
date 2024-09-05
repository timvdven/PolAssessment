using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Models.Dto;

namespace PolAssessment.AnprWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeController(IWebApiDbContext webApiDbContext) : ControllerBase
{
    private readonly IWebApiDbContext _webApiDbContext = webApiDbContext;

    [HttpGet]
    public ActionResult<WebUserResponse> Get()
    {
        var userId = User.GetUserId();
        var user = _webApiDbContext.WebUsers.Find(userId);
        var result = new WebUserResponse
        {
            Id = user!.Id,
            Fullname = user!.Fullname,
            Username = user!.Username,
            HttpResponseCode = HttpStatusCode.OK,
            Success = true
        };
        return Ok(result);
    }
}
