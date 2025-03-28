namespace Bl.FeatureFlag.Application.Commands.Identity.SendUserEmailConfirmationTokenToUser;

public record SendUserEmailConfirmationTokenToUserRequest(
    string Email)
    : IRequest<SendUserEmailConfirmationTokenToUserResponse>;