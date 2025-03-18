using Bl.FeatureFlag.Api.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Bl.FeatureFlag.Api.Endpoints;

internal static class IdentityEndpoints
{
    public static void MapEndpoints(
        IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapPost(
            "api/login",
            async (
                [FromBody] LoginRequestViewModel request,
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
                {
                    return Results.BadRequest("Email and password are required.");
                }

                var user = await userManager.FindByEmailAsync(request.Login);
                if (user == null)
                {
                    return Results.BadRequest("Invalid email or password.");
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    return Results.BadRequest("Invalid email or password.");
                }

                var token = GenerateJwtToken(user, configuration);

                return Results.Ok(new { Token = token });
            });
    }
}
