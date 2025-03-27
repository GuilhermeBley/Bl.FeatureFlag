using Bl.FeatureFlag.Api.Security;
using Bl.FeatureFlag.Application.Providers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bl.FeatureFlag.Api.Endpoints;

public static class FlagEndpoints
{
    public static void MapEndpoints(
        IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet(
            "api/subscription/{subscriptionId}/flag",
            async (
                Guid subscriptionId,
                HttpContext context,
                [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(
                    new Application.Commands.GetAllFlagsFromSubscription.GetAllFlagsFromSubscriptionRequest(
                        SubscriptionId: subscriptionId, 
                        UserId: context.User.RequiredUserId()));

                return Results.Ok();
            })
            .RequireAuthorization();

        endpointBuilder.MapPost(
            "api/subscription/{subscriptionId}/flag",
            async (
                Guid subscriptionId,
                [FromBody] Application.Commands.CreateFlag.CreateFlagRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                if (request.SubscriptionId != subscriptionId)
                    return Results.BadRequest(new
                    {
                        Error = "Invalid Subscription. Should be the same of the request."
                    });

                var response = await mediator.Send(request, cancellationToken);

                return Results.Created(
                    $"api/flag/subscription/{request.SubscriptionId}", 
                    new { FlagId = response.CreatedFlagId });
            })
            .RequireAuthorization();

        endpointBuilder.MapGet(
            "api/subscription/{subscriptionId}/flag/group/{groupName}/role/{roleName}",
            async (
                Guid subscriptionId,
                string groupName,
                string roleName,
                [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(
                    new Application.Commands.CheckFlagPermissions.CheckFlagPermissionsRequest(
                        GroupName: groupName,
                        RoleName: roleName));

                return Results.Ok(result);
            })
            .RequireAuthorization(b => b.AddAuthenticationSchemes(ClientRoleCheckSchema.Schema)
                .RequireRole(ClientRoleCheckSchema.RequiredRole));

        endpointBuilder.MapGet(
            "api/subscription",
            async (
                [FromServices] IMediator mediator,
                HttpContext context,
                int skip = 0,
                int take = 1000) =>
            {
                var response = await mediator.Send(
                    new Application.Commands.GetSubscriptionsFromUser.GetSubscriptionsFromUserRequest(
                        UserId: context.User.RequiredSubscriptionId(),
                        Skip: skip,
                        Take: take));

                return Results.Ok(response.Items);
            })
            .RequireAuthorization();

        endpointBuilder.MapPost(
            "api/subscription",
            async (
                [FromBody] Application.Commands.CreateSubscription.CreateSubscriptionRequest request,
                [FromServices] IMediator mediator,
                HttpContext context,
                int skip = 0,
                int take = 1000) =>
            {
                var response = await mediator.Send(request);

                return Results.Created($"api/subscription/{response.SubscriptionId}", new { response.SubscriptionId });
            })
            .RequireAuthorization();
    }
}
