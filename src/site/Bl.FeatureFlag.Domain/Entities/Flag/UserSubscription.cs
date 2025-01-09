namespace Bl.FeatureFlag.Domain.Entities.Flag;

public class UserSubscription
    : Entity
{
    public Guid SubscriptionId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private UserSubscription() { }

    public override bool Equals(object? obj)
    {
        return obj is UserSubscription subscription &&
               EntityId.Equals(subscription.EntityId) &&
               SubscriptionId.Equals(subscription.SubscriptionId) &&
               UserId.Equals(subscription.UserId) &&
               CreatedAt == subscription.CreatedAt;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId, SubscriptionId, UserId, CreatedAt);
    }

    public static UserSubscription Create(
        Guid subscriptionId,
        Guid userId,
        DateTime createdAt)
    {
        return new UserSubscription()
        {
            CreatedAt = createdAt,
            SubscriptionId = subscriptionId,
            UserId = userId
        };
    }
}
