using System.Text.RegularExpressions;

namespace Bl.FeatureFlag.Domain.Entities.Flag;

public class Subscription
    : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Subscription() { }

    public override bool Equals(object? obj)
    {
        return obj is UserSubscription subscription &&
               EntityId.Equals(subscription.EntityId) &&
               Name.Equals(subscription.Name) &&
               NormalizedName.Equals(subscription.NormalizedName) &&
               Id.Equals(subscription.SubscriptionId) &&
               CreatedAt == subscription.CreatedAt;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId, Id, Name, NormalizedName, CreatedAt);
    }

    public static Result<Subscription> Create(
        Guid id,
        Guid userId,
        string name,
        DateTime createdAt)
    {
        ResultBuilder<Subscription> builder = new();

        name = name ?? string.Empty;
        name = name.Trim().Replace("\n", "");
        var normalizedName = Regex.Match(name, "[0-9a-z]", RegexOptions.IgnoreCase | RegexOptions.Singleline)
            .Value
            .ToUpperInvariant();

        builder.AddIf(e => e.Name, string.IsNullOrWhiteSpace(normalizedName), CoreExceptionCode.BadRequest);

        return builder.CreateResult(() => new Subscription()
        {
            Name = name,
            NormalizedName = normalizedName,
            CreatedAt = createdAt,
            Id = id
        });
    }
}
