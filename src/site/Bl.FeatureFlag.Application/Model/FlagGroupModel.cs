using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Model;

public class FlagGroupModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserSubscriptionModel UserSubscription { get; set; } = null!;
    public List<FlagAccessModel> Flags { get; set; } = new();

    public static FlagGroupModel MapFromEntity(FlagGroup entity)
    {
        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            NormalizedName = entity.NormalizedName,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            UserSubscription = UserSubscriptionModel.MapFromEntity(entity.UserSubscription),
            Flags = entity.Flags.Select(e => FlagAccessModel.MapFromEntity(e)).ToList(),
        };
    }
}