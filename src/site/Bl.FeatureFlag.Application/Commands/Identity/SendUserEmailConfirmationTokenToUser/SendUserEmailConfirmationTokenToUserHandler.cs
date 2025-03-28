
using Bl.FeatureFlag.Application.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Bl.FeatureFlag.Application.Commands.Identity.SendUserEmailConfirmationTokenToUser;

public class SendUserEmailConfirmationTokenToUserHandler
    : IRequestHandler<SendUserEmailConfirmationTokenToUserRequest, SendUserEmailConfirmationTokenToUserResponse>
{
    private readonly UserManager<UserModel> _userManager;
    private readonly ILogger<SendUserEmailConfirmationTokenToUserHandler> _logger;

    public SendUserEmailConfirmationTokenToUserHandler(
        UserManager<UserModel> userManager,
        ILogger<SendUserEmailConfirmationTokenToUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<SendUserEmailConfirmationTokenToUserResponse> Handle(
        SendUserEmailConfirmationTokenToUserRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.LogInformation("User not found.");
            return new(SendUserEmailConfirmationTokenToUserResponseStatus.NotFound);
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        //
        // TODO: Send email with token
        //

        return new(SendUserEmailConfirmationTokenToUserResponseStatus.Success);
    }
}
