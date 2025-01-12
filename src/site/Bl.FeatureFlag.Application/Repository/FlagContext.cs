using Bl.FeatureFlag.Application.Model;

namespace Bl.FeatureFlag.Application.Repository;

public abstract class FlagContext
    : DbContext
{
    public DbSet<CompleteFlagAccessModel> Flags { get; set; }
    public DbSet<FlagGroupModel> FlagGroups { get; set; }
    public DbSet<UserSubscriptionModel> UserSubscriptions { get; set; }
}
