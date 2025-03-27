
using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Application.Repository;
using Bl.FeatureFlag.Domain.Entities.Flag;
using Microsoft.Extensions.Logging;

namespace Bl.FeatureFlag.Application.Commands.CreateSubscription;

public class CreateSubscriptionHandler
    : IRequestHandler<CreateSubscriptionRequest, CreateSubscriptionResponse>
{
    private readonly FlagContext _context;
    private readonly IClaimProvider _claimProvider;
    private readonly ILogger<CreateSubscriptionHandler> _logger;

    public CreateSubscriptionHandler(FlagContext context, IClaimProvider claimProvider, ILogger<CreateSubscriptionHandler> logger)
    {
        _context = context;
        _claimProvider = claimProvider;
        _logger = logger;
    }

    public async Task<CreateSubscriptionResponse> Handle(
        CreateSubscriptionRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating subscription {Name}", request.Name);

        var user = await _claimProvider.GetCurrentUserAsync();

        if (user.RequiredUserId() != request.UserId) throw new CoreException(CoreExceptionCode.Forbbiden);

        var subscriptionToAdd = Subscription.Create(
            Guid.NewGuid(),
            userId: request.UserId,
            name: request.Name,
            createdAt: DateTime.UtcNow)
            .RequiredResult;

        var hasDuplicatedSubscription = await _context.UserSubscriptions
            .Include(e => e.Subscription)
            .Where(e => e.UserId == request.UserId)
            .Where(s => s.Subscription.NormalizedName == subscriptionToAdd.NormalizedName)
            .AsNoTracking()
            .AnyAsync(cancellationToken);

        if (hasDuplicatedSubscription)
        {
            _logger.LogWarning("Subscription {Name} already exists", request.Name);
            throw new CoreException(CoreExceptionCode.DuplicatedSubscription);
        }

        var totalSubscriptions = await _context.UserSubscriptions
            .Where(e => e.UserId == request.UserId)
            .AsNoTracking()
            .CountAsync(cancellationToken);

        if (totalSubscriptions >= 50)
        {
            _logger.LogWarning("User {UserId} reached the maximum number of subscriptions", request.UserId);
            throw new CoreException(CoreExceptionCode.MaxSubscriptionsReached);
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var entityAddedResult = await _context.Subscriptions.AddAsync(SubscriptionModel.MapFromEntity(subscriptionToAdd), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _context.UserSubscriptions.AddAsync(new UserSubscriptionModel
        {
            SubscriptionId = entityAddedResult.Entity.Id,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _context.Database.CommitTransactionAsync();

        return new CreateSubscriptionResponse(entityAddedResult.Entity.Id);
    }
}
