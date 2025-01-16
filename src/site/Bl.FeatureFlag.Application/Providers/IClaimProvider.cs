using System.Security.Claims;

namespace Bl.FeatureFlag.Application.Providers;

public interface IClaimProvider
{
    Task<ClaimsPrincipal> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}
