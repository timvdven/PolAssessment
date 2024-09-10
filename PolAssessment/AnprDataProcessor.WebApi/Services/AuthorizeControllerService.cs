using Microsoft.EntityFrameworkCore;
using PolAssessment.AnprDataProcessor.WebApi.DbContexts;
using PolAssessment.Common.Lib.Models.DataProcessor;
using PolAssessment.Common.Lib.Services;

namespace PolAssessment.AnprDataProcessor.WebApi.Services;

public interface IAuthorizeControllerService
{
    Task<AuthorizeResponse> Authorize(AuthorizeRequest request);
}

public class AuthorizeControllerService(ILogger<AuthorizeControllerService> logger, IAnprDataDbContext anprDataDbContext, IAuthTokenHandler authTokenHandler, IHashService hashService) : IAuthorizeControllerService
{
    private readonly IAnprDataDbContext _anprDataDbContext = anprDataDbContext;
    private readonly IAuthTokenHandler _authTokenHandler = authTokenHandler;
    private readonly ILogger<AuthorizeControllerService> _logger = logger;
    private readonly IHashService _hashService = hashService;

    public async Task<AuthorizeResponse> Authorize(AuthorizeRequest request)
    {
        _logger.LogInformation("Authorizing request.");

        var clientId = request.ClientId;
        var hashedClientSecret = _hashService.GetHash(request.ClientSecret);

        var candidateUser = await _anprDataDbContext.UploadUsers.FirstOrDefaultAsync(x => x.ClientId == clientId && x.HashedClientSecret == hashedClientSecret);

        if (candidateUser == null)
        {
            _logger.LogWarning("Request unauthorized: invalid credentials.");
            return AuthorizeResponse.GetUnauthorizedResponse();
        }

        var token = _authTokenHandler.GenerateToken(candidateUser);
        _logger.LogInformation("Token generated successfully.");

        return AuthorizeResponse.GetSuccessResponse(token);
    }
}
