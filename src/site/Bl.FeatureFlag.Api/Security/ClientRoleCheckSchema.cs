namespace Bl.FeatureFlag.Api.Security;

internal class ClientRoleCheckSchema
{
    public static string RequiredRole = Domain.Security.UserClaim.SeeFlagsClientSide.Value;
}
