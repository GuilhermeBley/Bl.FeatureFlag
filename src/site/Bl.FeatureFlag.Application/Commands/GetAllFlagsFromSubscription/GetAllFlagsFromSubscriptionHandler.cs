
using Bl.FeatureFlag.Application.Repository;

namespace Bl.FeatureFlag.Application.Commands.GetAllFlagsFromSubscription;

public class GetAllFlagsFromSubscriptionHandler
    : IRequestHandler<GetAllFlagsFromSubscriptionRequest, GetAllFlagsFromSubscriptionResponse>
{
    private readonly FlagContext _context;
    private readonly IClaimProvider _claimProvider;

    public async Task<GetAllFlagsFromSubscriptionResponse> Handle(
        GetAllFlagsFromSubscriptionRequest request, 
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _claimProvider.GetCurrentUserAsync(cancellationToken);

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.SeeFlags);


    }
}
