using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Services;
using System.Data.Entity;

namespace PolAssessment.AnprDataProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthorizeController(AnprDataDbContext anprDataDbContext, IAuthTokenHandler authTokenHandler) : ControllerBase
    {
        private readonly AnprDataDbContext _anprDataDbContext = anprDataDbContext;
        private readonly IAuthTokenHandler _authTokenHandler = authTokenHandler;

        [HttpGet]
        public ActionResult<string> Get(
            [FromHeader(Name = "Client-Id")] string clientId,
            [FromHeader(Name = "Client-Secret")] string clientSecret)
        {
            var candidateUser = _anprDataDbContext.UploadUsers.FirstOrDefault(x => x.ClientId == clientId && x.ClientSecret == clientSecret);
            
            if (candidateUser == null)
            {
                return Unauthorized();
            }

            var token = _authTokenHandler.GenerateToken(candidateUser);

            return Ok(new { token });
        }
    }
}
