using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Model;

public class UserSubscriptionModel
{
    public Guid SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public static UserSubscriptionModel MapFromEntity(UserSubscription entity)
    {
        return new()
        {
            SubscriptionId = entity.SubscriptionId,
            UserId = entity.UserId,
            CreatedAt = entity.CreatedAt,
        };
    }
}

