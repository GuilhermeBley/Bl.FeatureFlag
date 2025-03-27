using System.Text.RegularExpressions;

namespace Bl.FeatureFlag.Domain.Entities.Flag;

public class UserSubscription
    : Entity
{
    public Guid Id { get; private set; }
    public Guid SubscriptionId { get; internal set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private UserSubscription() { }

    public override bool Equals(object? obj)
    {
        return obj is UserSubscription subscription &&
               EntityId.Equals(subscription.EntityId) &&
               Id.Equals(subscription.Id) &&
               SubscriptionId.Equals(subscription.SubscriptionId) &&
               UserId.Equals(subscription.UserId) &&
               CreatedAt == subscription.CreatedAt;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId, Id, SubscriptionId, UserId, CreatedAt);
    }

    public static UserSubscription Create(
        Guid id,
        Guid subscriptionId,
        Guid userId,
        DateTime createdAt)
    {
        return new UserSubscription()
        {
            Id = id,
            CreatedAt = createdAt,
            SubscriptionId = subscriptionId,
            UserId = userId
        };
    }
}
