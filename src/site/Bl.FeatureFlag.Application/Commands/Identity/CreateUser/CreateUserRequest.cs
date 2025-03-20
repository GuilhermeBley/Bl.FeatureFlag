namespace Bl.FeatureFlag.Application.Commands.Identity.CreateUser;

public record CreateUserRequest(
    string Email,
    string Password,
    string Name,
    string LastName,
    string? NickName)
    : IRequest<CreateUserResponse>;
