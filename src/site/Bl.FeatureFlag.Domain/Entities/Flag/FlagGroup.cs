﻿
using Bl.FeatureFlag.Domain.Extensions;
using Bl.FeatureFlag.Domain.Primitive;

namespace Bl.FeatureFlag.Domain.Entities.Flag;

public class FlagGroup
    : Entity,
    IGroupInfo
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; internal set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public UserSubscription UserSub { get; private set; } = null!;
    public IReadOnlyList<FlagAccess> Flags { get; private set; } = [];

    private FlagGroup() { }

    public override bool Equals(object? obj)
    {
        return obj is FlagGroup group &&
               EntityId.Equals(group.EntityId) &&
               Id.Equals(group.Id) &&
               Name == group.Name &&
               Description == group.Description &&
               UserSub.Equals(group.UserSub) &&
               CreatedAt == group.CreatedAt &&
               EqualityComparer<IReadOnlyList<FlagAccess>>.Default.Equals(Flags, group.Flags);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId, Id, Name, Description, UserSub, CreatedAt, Flags);
    }

    public void UpdateId(Guid id)
    {
        Id = id;
    }

    public static Result<FlagGroup> Create(
        Guid id,
        Guid userId,
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
            {
                var flagList = flags.ToList();

                var group = new FlagGroup
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Flags = flagList,
                    UserSub = UserSubscription.Create(Guid.Empty, subscriptionId, userId, createdAt),
                    CreatedAt = createdAt,
                    NormalizedName = name.RemoveAccents().Replace(" ", string.Empty),
                };

                flagList.ForEach(e => e.Group = group);

                return group;
            }
        );
    }
}
