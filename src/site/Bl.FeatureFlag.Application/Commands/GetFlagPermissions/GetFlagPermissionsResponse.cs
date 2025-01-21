using Bl.FeatureFlag.Application.Model;

namespace Bl.FeatureFlag.Application.Commands.GetFlagPermissions;

public record GetFlagPermissionsResponse(
    FlagAccessModel Flag);
