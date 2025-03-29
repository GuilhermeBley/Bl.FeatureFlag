namespace Bl.FeatureFlag.Application.Commands.Identity.ConfirmEmailByUserToken;

public record ConfirmEmailByUserTokenRequest(
    string Email,
    string Token)
    : IRequest<ConfirmEmailByUserTokenResponse>;
