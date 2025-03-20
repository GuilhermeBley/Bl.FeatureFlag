
using Bl.FeatureFlag.Domain.Primitive;
using Bl.FeatureFlag.Domain.Primitive.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Bl.FeatureFlag.Application.Commands.Identity.CreateUser;

public class CreateUserHandler
    : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly UserManager<IdentityUser<Guid>> userManager;

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            throw new CoreException(CoreExceptionCode.BadRequest, "Invalid Email.");
        }

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new CoreException(CoreExceptionCode.Conflict, "Email is already in use.");
        }

        var user = new IdentityUser<Guid>
        {
            UserName = request.Email,
            Email = request.Email,  
        };

        var createUserResult = await userManager.CreateAsync(user, request.Password);

        if (!createUserResult.Succeeded)
        {
            throw new AggregateCoreException(
                createUserResult.Errors.Select(e => new CoreException(e.Description)));
        }

        // TODO: Add claim
        // await userManager.AddClaimAsync(user, new Claim("custom-claim", "value"));

        existingUser = await userManager.FindByEmailAsync(request.Email)
            ?? throw new CoreException(CoreExceptionCode.BadRequest);
        return new(existingUser.Id);
    }
}
