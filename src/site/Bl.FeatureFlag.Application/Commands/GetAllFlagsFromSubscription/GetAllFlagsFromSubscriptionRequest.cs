namespace Bl.FeatureFlag.Application.Commands.GetAllFlagsFromSubscription;

public record GetAllFlagsFromSubscriptionRequest(
    Guid SubscriptionId,
    Guid UserId)
    : IRequest<GetAllFlagsFromSubscriptionResponse>;
