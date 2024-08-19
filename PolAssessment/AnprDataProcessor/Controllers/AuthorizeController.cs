using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PolAssessment.AnprDataProcessor.DbContexts;
using PolAssessment.AnprDataProcessor.Models;
using PolAssessment.AnprDataProcessor.Services;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public async Task<ActionResult<string>> Get()
        {
            Request.Headers.TryGetValue("Client-Id", out var clientIds);
            Request.Headers.TryGetValue("Client-Secret", out var clientSecrets);

            if (clientIds.Count == 0 || clientSecrets.Count == 0)
            {
                return Unauthorized();
            }

            var clientId = clientIds.FirstOrDefault();
            var clientSecret = clientSecrets.FirstOrDefault();

            var candidateUser = await _anprDataDbContext.UploadUsers.FirstOrDefaultAsync(x => x.ClientId == clientId && x.ClientSecret == clientSecret);
            if (candidateUser == null)
            {
                return Unauthorized();
            }

            var token = _authTokenHandler.GenerateToken(candidateUser);

            return Ok(new { token });
        }
    }
}
