
using Bl.FeatureFlag.Application.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Bl.FeatureFlag.Application.Commands.Identity.ConfirmEmailByUserToken;

public class ConfirmEmailByUserTokenHandler
    : IRequestHandler<ConfirmEmailByUserTokenRequest, ConfirmEmailByUserTokenResponse>
{
    private readonly UserManager<UserModel> _userManager;
    private readonly ILogger<ConfirmEmailByUserTokenHandler> _logger;

    public async Task<ConfirmEmailByUserTokenResponse> Handle(
        ConfirmEmailByUserTokenRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            _logger.LogInformation("User not found for email {Email}", request.Email);
            return new(ConfirmEmailByUserTokenResponseStatus.NotFound);
        }

        var alreadyConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (alreadyConfirmed)
        {
            _logger.LogInformation("User already confirmed email {Email}", request.Email);
            return new(ConfirmEmailByUserTokenResponseStatus.AlreadyConfirmed);
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded)
        {
            _logger.LogInformation("User confirmed email {Email}", request.Email);
            return new(ConfirmEmailByUserTokenResponseStatus.Confirmed);
        }

        _logger.LogWarning("Error confirming email {Email}. Error {error}", request.Email, result.Errors);

        return new(ConfirmEmailByUserTokenResponseStatus.Error);
    }
}
