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
                [FromBody] Application.Commands.Identity.Login.LoginRequest request,
                [FromServices] IMediator mediator,
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager,
                IJwtTokenService jwtTokenService,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(request, cancellationToken);

                var token = jwtTokenService.GenerateTokenAsync(response.Claims.ToArray(), TimeSpan.FromMinutes(30), cancellationToken);

                return Results.Ok(new { Token = token });
            });

        endpointBuilder.MapPost(
            "api/user",
            async ([FromBody] Application.Commands.Identity.CreateUser.CreateUserRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(request, cancellationToken);

                return Results.Created($"api/user/{response.UserId}", new{ response.UserId });
            });
    }
}
