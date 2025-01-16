using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Model;

public class FlagAccessModel
{
    public Guid GroupId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string NormalizedRoleName { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public static FlagAccessModel MapFromEntity(
        FlagAccess entity)
    {
        return new()
        {
            Active = entity.Active,
            ExpiresAt = entity.ExpiresAt,
            CreatedAt = entity.CreatedAt,
            NormalizedRoleName = entity.NormalizedRoleName,
            RoleName = entity.RoleName,
        };
    }
}
