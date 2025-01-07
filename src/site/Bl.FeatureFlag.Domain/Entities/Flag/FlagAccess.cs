using Bl.FeatureFlag.Domain.Extensions;
using Bl.FeatureFlag.Domain.Primitive;

namespace Bl.FeatureFlag.Domain.Entities.Flag;

/// <summary>
/// Represents an item that can be checked if has or not the access.
/// </summary>
public class FlagAccess
    : Entity
{
    public string RoleName { get; internal set; } = string.Empty;
    public string NormalizedRoleName { get; internal set; } = string.Empty;
    public bool Active { get; internal set; }
    public DateTime? ExpiresAt { get; internal set; }
    public DateTime CreatedAt { get; internal set; }

    internal FlagAccess() { }

    public override bool Equals(object? obj)
    {
        return obj is FlagAccess access &&
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

    public static Result<FlagAccess> Create(
        string roleName,
        bool active,
        DateTime? expiresAt,
        DateTime createdAt)
    {
        ResultBuilder<FlagAccess> builder = new();

        roleName = roleName ?? string.Empty;

        builder.AddIf(
            e => e.RoleName,
            roleName.Length < 3 || roleName.Length > 255,
            CoreExceptionCode.InvalidStringLength);

        return builder.CreateResult(() =>
            new FlagAccess
            {
                Active = active,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt,
                NormalizedRoleName = roleName.RemoveAccents().Replace(" ", string.Empty),
                RoleName = roleName,
            }
        );
    }
}
