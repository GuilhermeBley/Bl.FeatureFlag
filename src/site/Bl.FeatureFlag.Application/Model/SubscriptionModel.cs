using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Model;

public class SubscriptionModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public static SubscriptionModel MapFromEntity(Subscription entity)
    {
        return new SubscriptionModel
        {
            Id = entity.Id,
            Name = entity.Name,
            NormalizedName = entity.NormalizedName,
            CreatedAt = entity.CreatedAt
        };
    }
}
