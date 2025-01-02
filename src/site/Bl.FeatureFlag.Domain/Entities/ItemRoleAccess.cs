using Bl.FeatureFlag.Domain.Primitive;

namespace Bl.FeatureFlag.Domain.Entities;

public class ItemRoleAccess
    : Entity
{
    public string RoleName { get; private set; }
    public string NormalizedRoleName { get; private set; }
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

        throw new NotImplementedException();
    }
}
