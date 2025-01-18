using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Model;

public class FlagAccessModel
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string NormalizedRoleName { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public static FlagAccessModel MapFromEntity(
        FlagAccess entity,
        Guid groupId)
    {
        return new()
        {
            Id = entity.Id,
            GroupId = groupId,
            Active = entity.Active,
            ExpiresAt = entity.ExpiresAt,
            CreatedAt = entity.CreatedAt,
            NormalizedRoleName = entity.NormalizedRoleName,
            RoleName = entity.RoleName,
        };
    }
}
