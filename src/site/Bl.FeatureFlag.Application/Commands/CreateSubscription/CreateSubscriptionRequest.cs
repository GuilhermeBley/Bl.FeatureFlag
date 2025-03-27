namespace Bl.FeatureFlag.Application.Commands.CreateSubscription;

public record CreateSubscriptionRequest(
    Guid UserId,
    string Name)
    : IRequest<CreateSubscriptionResponse>;
