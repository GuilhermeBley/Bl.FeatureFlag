using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Application.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bl.FeatureFlag.Infrastructure.Repository;

internal class PostgreFlagContext
    : FlagContext
{
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CompleteFlagAccessModel>(b =>
        {
            b.HasKey(e => e.Id);

            b.HasIndex(e => new { e.GroupId, e.NormalizedRoleName }).IsUnique();

            b.Property(e => e.Active).IsRequired();
            b.Property(e => e.Description).IsRequired();

            b.Property(e => e.GroupId).IsRequired();
            b.Property(e => e.RoleName).IsRequired().HasMaxLength(255);
            b.Property(e => e.NormalizedRoleName).IsRequired().HasMaxLength(255);
            b.Property(e => e.Active).IsRequired();
            b.Property(e => e.ExpiresAt);
            b.Property(e => e.CreatedAt).IsRequired();
            b.Property(e => e.Description);
            b.Property(e => e.Obs);

            b.HasOne<FlagGroupModel>()
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
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
