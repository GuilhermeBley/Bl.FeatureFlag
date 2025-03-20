namespace Bl.FeatureFlag.Application.Commands.Identity.Login;

public record LoginRequest(
    string Login,
    string Password)
    : IRequest<LoginResponse>;
