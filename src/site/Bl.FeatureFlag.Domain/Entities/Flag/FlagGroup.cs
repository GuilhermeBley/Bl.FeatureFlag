
using Bl.FeatureFlag.Domain.Extensions;
using Bl.FeatureFlag.Domain.Primitive;

namespace Bl.FeatureFlag.Domain.Entities.Flag;

public class FlagGroup
    : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedRoleName { get; internal set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyList<FlagAccess> Flags { get; private set; } = [];

    private FlagGroup() { }

    public override bool Equals(object? obj)
    {
        return obj is FlagGroup group &&
               EntityId.Equals(group.EntityId) &&
               Id.Equals(group.Id) &&
               Name == group.Name &&
               Description == group.Description &&
               SubscriptionId.Equals(group.SubscriptionId) &&
               CreatedAt == group.CreatedAt &&
               EqualityComparer<IReadOnlyList<FlagAccess>>.Default.Equals(Flags, group.Flags);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId, Id, Name, Description, SubscriptionId, CreatedAt, Flags);
    }

    public static Result<FlagGroup> Create(
        Guid id,
        string name,
        string? description,
        FlagAccess[] flags,
        Guid subscriptionId,
        DateTime createdAt)
    {
        ResultBuilder<FlagGroup> builder = new();

        name = name ?? string.Empty;

        builder.AddIf(
            e => e.Name,
            name.Length < 3 || name.Length > 255,
            CoreExceptionCode.InvalidStringLength);

        builder.AddIf(
            e => e.Flags,
            flags.Where(f => f is not null).Count() == 0,
            CoreExceptionCode.BadRequest);

        return builder.CreateResult(() =>
            new FlagGroup
            {
                Id = id,
                Name = name,
                Description = description,
                Flags = flags.ToList(),
                Id = subscriptionId,
                SubscriptionId = subscriptionId,
                CreatedAt = createdAt,
                NormalizedRoleName = name.RemoveAccents().Replace(" ", string.Empty),
            }
        );
    }
}
