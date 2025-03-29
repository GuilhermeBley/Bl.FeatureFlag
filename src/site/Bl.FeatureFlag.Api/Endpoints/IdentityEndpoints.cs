using Bl.FeatureFlag.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        endpointBuilder.MapPost(
            "api/user/request/user-email-confirmation",
            async ([FromBody] Application.Commands.Identity.SendUserEmailConfirmationTokenToUser.SendUserEmailConfirmationTokenToUserRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(request, cancellationToken);

                return Results.Ok();
            });

        endpointBuilder.MapPost(
            "api/user/confirm/user-email-confirmation",
            async ([FromBody] Application.Commands.Identity.ConfirmEmailByUserToken.ConfirmEmailByUserTokenRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(request, cancellationToken);

                return Results.Ok();
            });
    }
}
