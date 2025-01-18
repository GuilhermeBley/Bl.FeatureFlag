using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Application.Repository;
using Bl.FeatureFlag.Domain.Entities.Flag;
using System.Xml.Linq;

namespace Bl.FeatureFlag.Application.Commands.CreateFlag;

public class CreateFlagHandler
    : IRequestHandler<CreateFlagRequest, CreateFlagResponse>
{
    private readonly FlagContext _context;
    private readonly IClaimProvider _claimProvider;
    private readonly IFastFlagRepository _fastFlagRepository;

    public CreateFlagHandler(
        FlagContext context, 
        IClaimProvider claimProvider, 
        IFastFlagRepository fastFlagRepository)
    {
        _context = context;
        _claimProvider = claimProvider;
        _fastFlagRepository = fastFlagRepository;
    }

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

        var groupAdded = await _context
            .FlagGroups
            .Where(e => e.NormalizedName == group.NormalizedName)
            .FirstOrDefaultAsync(cancellationToken);

        if (groupAdded is not null &&
            request.GroupDescription is not null)
            groupAdded.Description = group.Description;

        if (groupAdded is null)
        {
            groupAdded = (await _context
                .FlagGroups
                .AddAsync(FlagGroupModel.MapFromEntity(group), cancellationToken))
                .Entity;
        }

        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            group.UpdateId(groupAdded.Id);

            foreach (var flag in group.Flags.Cast<CompleteFlagAccess>())
            {
                var flagAlreadyRegistered = await (
                    from f in _context.Flags
                    join g in _context.FlagGroups
                        on f.GroupId equals g.Id
                    where f.NormalizedRoleName == flag.NormalizedRoleName
                    where g.NormalizedName == @group.NormalizedName
                    select new { f.Id })
                    .AnyAsync(cancellationToken);

                if (flagAlreadyRegistered) continue;

                await _context
                    .Flags
                    .AddAsync(CompleteFlagAccessModel.MapFromEntity(flag, group.Id), cancellationToken);

                // Add flag to another data source, like azure data tables
                await _fastFlagRepository.AddAsync(flag, cancellationToken);
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new(group.Id);
    }
}
