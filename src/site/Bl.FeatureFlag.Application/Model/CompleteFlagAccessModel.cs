using Bl.FeatureFlag.Domain.Entities.Flag;
using System.Text.RegularExpressions;

namespace Bl.FeatureFlag.Application.Model;

public class CompleteFlagAccessModel
    : FlagAccessModel
{
    public string? Description { get; set; }
    public string? Obs { get; set; }

    public static CompleteFlagAccessModel MapFromEntity(
        CompleteFlagAccess entity,
        Guid groupId)
    {
        return new()
        {
            Id = entity.Id,
            GroupId = groupId,
            RoleName = entity.RoleName,
            NormalizedRoleName = entity.NormalizedRoleName,
            Active = entity.Active,
            CreatedAt = entity.CreatedAt,
            Description = entity.Description,
            Obs = entity.Obs,
            ExpiresAt = entity.ExpiresAt,
        };
    }
}
