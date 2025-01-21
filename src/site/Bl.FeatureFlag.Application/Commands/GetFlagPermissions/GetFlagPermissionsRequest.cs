namespace Bl.FeatureFlag.Application.Commands.GetFlagPermissions;

public record GetFlagPermissionsRequest(
    string GroupName,
    string RoleName)
    : IRequest<GetFlagPermissionsResponse>;
