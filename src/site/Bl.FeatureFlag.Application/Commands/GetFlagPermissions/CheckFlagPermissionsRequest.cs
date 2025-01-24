namespace Bl.FeatureFlag.Application.Commands.CheckFlagPermissions;

public record CheckFlagPermissionsRequest(
    string GroupName,
    string RoleName,
    IDateTimeProvider? TimeProvider = null)
    : IRequest<CheckFlagPermissionsResponse>;
