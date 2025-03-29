namespace Bl.FeatureFlag.Application.Commands.Identity.ConfirmEmailByUserToken;

public enum ConfirmEmailByUserTokenResponseStatus
{
    NotFound,
    AlreadyConfirmed,
    Confirmed,
    Error,
}

public record ConfirmEmailByUserTokenResponse(
    ConfirmEmailByUserTokenResponseStatus Status);
