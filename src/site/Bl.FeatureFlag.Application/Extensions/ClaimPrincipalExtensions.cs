using System.Security.Claims;

namespace Bl.FeatureFlag.Application.Extensions;

public static class ClaimPrincipalExtensions
{
    /// <summary>
    /// Get the user ID, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static Guid RequiredSubscriptionId(this ClaimsPrincipal principal)
        => GetSubscriptionId(principal)
        ?? throw new CoreException(CoreExceptionCode.Forbbiden);

    /// <summary>
    /// Get the subscription ID or null.
    /// </summary>
    public static Guid? GetSubscriptionId(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_SUBSCRIPTION_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var id))
            return null;

        return id;
    }

    /// <summary>
    /// Get the user ID, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static Guid RequiredUserId(this ClaimsPrincipal principal)
        => GetUserId(principal)
        ?? throw new UnauthorizedAccessException();

    /// <summary>
    /// Get the user ID or null.
    /// </summary>
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_USER_ID);

        if (claim is null ||
            !Guid.TryParse(claim.Value, out var id))
            return null;

        return id;
    }

    /// <summary>
    /// Get the user email, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string RequiredUserEmail(this ClaimsPrincipal principal)
        => GetUserEmail(principal)
        ?? throw new UnauthorizedAccessException();

    /// <summary>
    /// Get the user email or null.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_USER_NAME);

        if (claim is null)
            return null;

        return claim.Value;
    }

    /// <summary>
    /// Get the user name, if null, it throws an exception.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string RequiredUserName(this ClaimsPrincipal principal)
        => GetUserName(principal)
        ?? throw new UnauthorizedAccessException();

    /// <summary>
    /// Get the user name or null.
    /// </summary>
    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        var claim = principal
            .Claims
            .FirstOrDefault(claim => claim.Type == Domain.Security.UserClaim.DEFAULT_USER_NAME);

        if (claim is null)
            return null;

        return claim.Value;
    }

    public static bool IsInRole(this ClaimsPrincipal principal, Claim roleClaim)
    {
        if (principal.Identities.Any(id => id.RoleClaimType == roleClaim.Type))
            return false;

        return principal.IsInRole(roleClaim.Value);
    }

    /// <summary>
    /// This method checks if the user is logged and if it contains the role.
    /// </summary>
    /// <exception cref="UnauthorizedCoreException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void ThrowIfIsNotInSubscription(this ClaimsPrincipal principal, Guid subscriptionId)
    {
        ThrowIfIsntLogged(principal);

        if (!principal.Requireds)
            throw new CoreException(CoreExceptionCode.Forbbiden);
    }

    /// <summary>
    /// This method checks if the user is logged and if it contains the role.
    /// </summary>
    /// <exception cref="UnauthorizedCoreException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void ThrowIfDoesntContainRole(this ClaimsPrincipal principal, Claim roleClaim)
    {
        ThrowIfIsntLogged(principal);

        if (!principal.IsInRole(roleClaim.Value))
            throw new CoreException(CoreExceptionCode.Forbbiden);
    }

    /// <summary>
    /// This method checks if the user is logged and if it contains the role.
    /// </summary>
    /// <exception cref="UnauthorizedCoreException"></exception>
    /// <exception cref="ForbbidenCoreException"></exception>
    public static void ThrowIfDoesntContainRole(this ClaimsPrincipal principal, string role)
    {
        ThrowIfIsntLogged(principal);

        if (!principal.IsInRole(role))
            throw new CoreException(CoreExceptionCode.Forbbiden);
    }

    public static void ThrowIfIsntLogged(this ClaimsPrincipal principal)
    {
        if (!IsLogged(principal))
            throw new CoreException(CoreExceptionCode.Unauthorized);
    }

    public static bool IsLogged(this ClaimsPrincipal principal)
    {
        return principal.HasClaim(p => p.Type == Domain.Security.UserClaim.DEFAULT_USER_ID);
    }
}
