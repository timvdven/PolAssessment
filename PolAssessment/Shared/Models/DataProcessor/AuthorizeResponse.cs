using System.Net;

namespace PolAssessment.Shared.Models.DataProcessor;

public class AuthorizeResponse : BaseWebResponse
{
    public AccessToken? AccessToken { get; set; }

    public static AuthorizeResponse GetSuccessResponse(AccessToken accessToken)
    {
        return new AuthorizeResponse
        {
            AccessToken = accessToken,
            Success = true,
            HttpResponseCode = HttpStatusCode.OK
        };
    }

    public static AuthorizeResponse GetUnauthorizedResponse(string? message = null)
    {
        return new AuthorizeResponse
        {
            Success = false,
            AccessToken = null,
            HttpResponseCode = HttpStatusCode.Unauthorized,
            ErrorMessage = message
        };
    }
}
