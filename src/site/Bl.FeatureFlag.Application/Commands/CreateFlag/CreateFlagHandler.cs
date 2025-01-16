using Bl.FeatureFlag.Application.Repository;
using Bl.FeatureFlag.Domain.Entities.Flag;
using System.Xml.Linq;

namespace Bl.FeatureFlag.Application.Commands.CreateFlag;

public class CreateFlagHandler
    : IRequestHandler<CreateFlagRequest, CreateFlagResponse>
{
    private readonly FlagContext _context;
    private readonly IClaimProvider _claimProvider;

    public async Task<CreateFlagResponse> Handle(
        CreateFlagRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _claimProvider.GetCurrentUserAsync(cancellationToken);

        var userId = user.RequiredUserId();

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.SeeFlags);

        var isSubscriptionRegisteredInUser =
            await _context
            .UserSubscriptions
            .AsNoTracking()
            .Where(e => e.SubscriptionId == request.SubscriptionId)
            .Where(e => e.UserId == userId)
            .AnyAsync(cancellationToken);

        if (!isSubscriptionRegisteredInUser) throw new CoreException(CoreExceptionCode.Forbbiden);

        var flags = request.Items.Select(
            e => CompleteFlagAccess.Create(
                roleName: e.Name,
                description: e.Description ?? string.Empty,
                obs: string.Empty,
                active: true,
                expiresAt: e.ExpiresAt,
                createdAt: DateTime.UtcNow)
                .RequiredResult)
            .ToArray();

        var group = FlagGroup.Create(
            id: Guid.Empty,
            userId: userId,
            name: request.GroupName,
            description: request.GroupDescription,
            flags: flags,
            subscriptionId: request.SubscriptionId,
            createdAt: DateTime.UtcNow)
            .RequiredResult;

        await using var transaction
            = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var flag in group.Flags)
            {
                var flagAlreadyRegistered = await (
    
                    )
                    .AnyAsync(e => e.NormalizedRoleName == flag.NormalizedRoleName, cancellationToken);

                if (flagAlreadyRegistered) throw new CoreException(CoreExceptionCode.Conflict);
            }

            // Add flag to another data source, like azure data tables

            _context.fla
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
