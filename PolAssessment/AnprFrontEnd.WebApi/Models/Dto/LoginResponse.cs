using System.Net;
using PolAssessment.Common.Lib.Models;

namespace PolAssessment.AnprWebApi.Models.Dto;

public class LoginResponse : BaseWebResponse
{
    public AccessToken? AccessToken { get; set; }

    public static LoginResponse GetSuccessResponse(AccessToken accessToken)
    {
        return new LoginResponse
        {
            AccessToken = accessToken,
            Success = true,
            HttpResponseCode = HttpStatusCode.OK
        };
    }

    public static LoginResponse GetUnauthorizedResponse(string? message = null)
    {
        return new LoginResponse
        {
            Success = false,
            AccessToken = null,
            HttpResponseCode = HttpStatusCode.Unauthorized,
            ErrorMessage = message
        };
    }
}
