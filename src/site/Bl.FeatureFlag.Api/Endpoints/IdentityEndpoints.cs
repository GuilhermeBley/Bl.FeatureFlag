using Bl.FeatureFlag.Api.Model;
using Bl.FeatureFlag.Api.Services;
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
                IJwtTokenService jwtTokenService,
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

                var claims = await userManager.GetClaimsAsync(user);

                var token = jwtTokenService.GenerateTokenAsync(claims.ToArray(), TimeSpan.FromMinutes(30),cancellationToken);

                return Results.Ok(new { Token = token });
            });

        endpointBuilder.MapPost(
            "api/user",
            async ([FromBody] CreateUserRequestViewModel request,
                UserManager<IdentityUser> userManager,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return Results.BadRequest("Email and password are required.");
                }

                var existingUser = await userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return Results.BadRequest("User with this email already exists.");
                }

                var user = new IdentityUser
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                var createUserResult = await userManager.CreateAsync(user, request.Password);

                if (!createUserResult.Succeeded)
                {
                    return Results.BadRequest(createUserResult.Errors);
                }

                // TODO: Add claim
                // await userManager.AddClaimAsync(user, new Claim("custom-claim", "value"));

                return Results.Ok(new { Message = "User created successfully.", UserId = user.Id });
            });
    }
}
