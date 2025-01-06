using Bl.FeatureFlag.Domain.Extensions;
using Bl.FeatureFlag.Domain.Primitive;

namespace Bl.FeatureFlag.Domain.Entities;

/// <summary>
/// Represents an access item that can be checked if has or not the access.
/// </summary>
public class ItemRoleAccess
    : Entity
{
    public string RoleName { get; private set; } = string.Empty;
    public string NormalizedRoleName { get; private set; } = string.Empty;
    public bool Active { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ItemRoleAccess() { }

    public override bool Equals(object? obj)
    {
        return obj is ItemRoleAccess access &&
               EntityId.Equals(access.EntityId) &&
               RoleName == access.RoleName &&
               NormalizedRoleName == access.NormalizedRoleName &&
               Active == access.Active &&
               ExpiresAt == access.ExpiresAt &&
               CreatedAt == access.CreatedAt;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId, RoleName, NormalizedRoleName, Active, ExpiresAt, CreatedAt);
    }

    public bool CanAccess(IDateTimeProvider? provider = null)
    {
        provider = provider ?? DateTimeProvider.Default;

        return Active && provider.UtcNow > ExpiresAt;
    }

    public static Result<ItemRoleAccess> Create(
        string roleName,
        bool active,
        DateTime? expiresAt,
        DateTime createdAt)
    {
        ResultBuilder<ItemRoleAccess> builder = new();

        roleName = roleName ?? string.Empty;

        builder.AddIf(
            e => e.RoleName,
            roleName.Length < 3 || roleName.Length > 255, 
            CoreExceptionCode.InvalidStringLength);

        return builder.CreateResult(() =>
            new ItemRoleAccess
            {
                Active = active,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt,
                NormalizedRoleName = roleName.RemoveAccents(),
                RoleName = roleName,
            }
        );
    }
}
