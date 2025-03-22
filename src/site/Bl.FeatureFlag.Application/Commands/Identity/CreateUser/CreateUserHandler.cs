
using Bl.FeatureFlag.Application.Model.Identity;
using Bl.FeatureFlag.Domain.Primitive;
using Bl.FeatureFlag.Domain.Primitive.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Bl.FeatureFlag.Application.Commands.Identity.CreateUser;

public class CreateUserHandler
    : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly UserManager<UserModel> _userManager;

    public CreateUserHandler(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            throw new CoreException(CoreExceptionCode.BadRequest, "Invalid Email.");
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new CoreException(CoreExceptionCode.Conflict, "Email is already in use.");
        }

        var user = new UserModel
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false,
            LastName = request.LastName,
            LockoutEnabled = true,
            LockoutEnd = null,
            Name = request.Name,
            NickName = request.NickName
        };

        var createUserResult = await _userManager.CreateAsync(user, request.Password);

        if (!createUserResult.Succeeded)
        {
            throw new AggregateCoreException(
                createUserResult.Errors.Select(e => new CoreException(e.Description)));
        }

        existingUser = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new CoreException(CoreExceptionCode.BadRequest);

        return new(existingUser.Id);
    }
}
