using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace Bl.FeatureFlag.Infrastructure.Repository;

internal class PostgreFlagContext
    : FlagContext
{
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CompleteFlagAccessModel>(b =>
        {

        });

        builder.Entity<FlagGroupModel>(b =>
        {

        });

        builder.Entity<UserSubscriptionModel>(b =>
        {

        });

        builder.Entity<SubscriptionModel>(b =>
        {

        });
    }
}
