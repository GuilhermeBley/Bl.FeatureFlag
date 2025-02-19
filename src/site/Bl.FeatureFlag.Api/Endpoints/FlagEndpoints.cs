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
    }
}
