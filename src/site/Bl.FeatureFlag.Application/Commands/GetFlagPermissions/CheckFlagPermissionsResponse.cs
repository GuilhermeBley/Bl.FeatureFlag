using Bl.FeatureFlag.Application.Model;

namespace Bl.FeatureFlag.Application.Commands.CheckFlagPermissions;

public record CheckFlagPermissionsResponse(
    bool CanAccessArea,
    bool IsAreaRegistered);
