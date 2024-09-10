using System.Security.Authentication;
using System.Security.Claims;

namespace PolAssessment.AnprDataProcessor.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var trial = int.TryParse(claim, out var userId);
        if (!trial)
        {
            throw new InvalidCredentialException("Invalid user claim");
        }

        return userId;
    }
}
