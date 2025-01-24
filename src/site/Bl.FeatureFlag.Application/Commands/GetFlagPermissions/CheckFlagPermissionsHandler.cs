using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Application.Repository;

namespace Bl.FeatureFlag.Application.Commands.CheckFlagPermissions;

public class CheckFlagPermissionsHandler
    : IRequestHandler<CheckFlagPermissionsRequest, CheckFlagPermissionsResponse>
{
    private readonly IClaimProvider _claimProvider;
    private readonly IFastFlagRepository _repository;

    public CheckFlagPermissionsHandler(IClaimProvider claimProvider, IFastFlagRepository repository)
    {
        _claimProvider = claimProvider;
        _repository = repository;
    }

    public async Task<CheckFlagPermissionsResponse> Handle(
        CheckFlagPermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _claimProvider.GetCurrentUserAsync();

        var subscriptionId = user.RequiredSubscriptionId();

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.SeeFlags);

        var flagFound = await _repository.GetByNameAsync(subscriptionId, request.GroupName, request.RoleName, cancellationToken);

        if (flagFound is null)
        {
            return new(CanAccessArea: false, IsAreaRegistered: false);
        }

        var canAccess = !IsFlagExpired(flagFound, request.TimeProvider) &&
            flagFound.Active;

        return new(CanAccessArea: canAccess , IsAreaRegistered: true);
    }

    private static bool IsFlagExpired(FlagAccessModel flag, IDateTimeProvider? provider)
    {
        provider = provider ?? DateTimeProvider.Default;


        return flag.ExpiresAt is not null &&
            flag.ExpiresAt < provider.UtcNow;
    }
}
