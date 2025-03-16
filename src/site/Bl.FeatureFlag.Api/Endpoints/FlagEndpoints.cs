using Bl.FeatureFlag.Application.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Bl.FeatureFlag.Api.Endpoints;

public static class FlagEndpoints
{
    public static void MapEndpoints(
        IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet(
            "api/flag/subscription/{subscriptionId}",
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
            });

        endpointBuilder.MapPost(
            "api/flag/subscription/{subscriptionId}",
            async (
                Guid subscriptionId,
                [FromBody] Application.Commands.CreateFlag.CreateFlagRequest request,
                HttpContext context,
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
            });
    }
}
