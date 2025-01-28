
using Bl.FeatureFlag.Application.Providers;
using Bl.FeatureFlag.Application.Repository;
using System.Xml.Linq;

namespace Bl.FeatureFlag.Application.Commands.GetSubscriptionsFromUser;

public class GetSubscriptionsFromUserHandler
    : IRequestHandler<GetSubscriptionsFromUserRequest, GetSubscriptionsFromUserResponse>
{
    private readonly FlagContext _context;
    private readonly IClaimProvider _claimProvider;

    public GetSubscriptionsFromUserHandler(FlagContext context, IClaimProvider claimProvider)
    {
        _context = context;
        _claimProvider = claimProvider;
    }

    public async Task<GetSubscriptionsFromUserResponse> Handle(
        GetSubscriptionsFromUserRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _claimProvider.GetCurrentUserAsync(cancellationToken);

        var userId = user.RequiredUserId();

        if (userId != request.UserId)
        {
            throw new CoreException(CoreExceptionCode.Unauthorized);
        }

        var items = await _context
            .UserSubscriptions
            .Include(e => e.Subscription)
            .AsNoTracking()
            .Where(e => e.UserId == request.UserId)
            .Select(e => new GetSubscriptionsFromUserItem(
                /*Id:*/ e.Subscription.Id,
                /*Name:*/ e.Subscription.Name,
                /*NormalizedName:*/ e.Subscription.NormalizedName,
                /*CreatedAt:*/ e.Subscription.CreatedAt))
            .Skip(request.Skip)
            .Take(request.Take)
            .ToArrayAsync(cancellationToken);

        return new(request.UserId, items);
    }
}
