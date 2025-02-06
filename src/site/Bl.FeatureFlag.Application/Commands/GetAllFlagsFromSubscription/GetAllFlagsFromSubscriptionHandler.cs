
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

        var userId = user.RequiredUserId();

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.SeeFlags);

        var items = await
            (from usub in _context.UserSubscriptions
             join sub in _context.Subscriptions
                 on new { usub.SubscriptionId, usub.UserId } equals new { SubscriptionId = sub.Id, UserId = userId }
             join gro in _context.FlagGroups
                on sub.Id equals gro.SubscriptionId
             join flag in _context.Flags
                 on gro.Id equals flag.GroupId
             select new
             {
                 SubscriptionCreatedAt = sub.CreatedAt,
                 SubscriptionId = userId,
                 SubscriptionNormalizedName = sub.NormalizedName,
                 SubscriptionName = sub.Name,
                 flag.Active,
                 flag.CreatedAt,
                 flag.Description,
                 flag.ExpiresAt,
                 flag.GroupId,
                 flag.Id,
                 flag.NormalizedRoleName,
                 flag.RoleName,
                 GroupNormalizedName = gro.NormalizedName,
                 GroupName = gro.Name,
             })
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        Dictionary<Guid, GetAllFlagsFromSubscriptionSubInfo> subscriptionData = new();
        foreach (var item in items)
        {
            if (!subscriptionData.TryGetValue(item.SubscriptionId, out var subInfo))
            {
                subInfo = new()
                {
                    Id = item.SubscriptionId,
                    CreatedAt = item.SubscriptionCreatedAt,
                    Name = item.SubscriptionName,
                    NormalizedName = item.SubscriptionNormalizedName,
                };

                subscriptionData.Add(item.SubscriptionId, subInfo);
            }

            subInfo.Flags.Add(new GetAllFlagsFromSubscriptionFlagInfo()
            {
                Active = item.Active,
                ExpiresAt = item.ExpiresAt,
                FlagCreatedAt = item.CreatedAt,
                FlagId = item.Id,
                GroupName = item.GroupName,
                NormalizedGroupName = item.GroupNormalizedName,
            });
        }

        return new(subscriptionData.Values.ToArray());
    }
}
