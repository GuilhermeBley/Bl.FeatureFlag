namespace Bl.FeatureFlag.Application.Commands.Identity.SendUserEmailConfirmationTokenToUser;



public enum SendUserEmailConfirmationTokenToUserResponseStatus
{
    NotFound,
    Success,
    FailedToSendEmail
}

public record SendUserEmailConfirmationTokenToUserResponse(SendUserEmailConfirmationTokenToUserResponseStatus status);
