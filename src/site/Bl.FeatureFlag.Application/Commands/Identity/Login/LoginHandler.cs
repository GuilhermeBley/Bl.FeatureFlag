
using Bl.FeatureFlag.Application.Model.Identity;
using Bl.FeatureFlag.Domain.Primitive;
using Microsoft.AspNetCore.Identity;

namespace Bl.FeatureFlag.Application.Commands.Identity.Login;

public class LoginHandler
    : IRequestHandler<LoginRequest, LoginResponse>
{
    public readonly UserManager<UserModel> _userManager;

    public LoginHandler(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
        {
            throw new CoreException(CoreExceptionCode.BadRequest, "Invalid login.");
        }

        var user = await _userManager.FindByEmailAsync(request.Login);
        if (user == null)
        {
            throw new CoreException(CoreExceptionCode.Unauthorized, "Invalid login or password.");
        }
        
        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
        {
            throw new CoreException(CoreExceptionCode.Unauthorized, "Invalid login or password.");
        }

        var claims = await _userManager.GetClaimsAsync(user);

        return new(user.Id, claims.ToArray());
    }
}
