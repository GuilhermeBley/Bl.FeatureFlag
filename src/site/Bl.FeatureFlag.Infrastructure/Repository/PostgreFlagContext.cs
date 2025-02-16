using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Application.Repository;
using Bl.FeatureFlag.Domain.Entities.Flag;
using Bl.FeatureFlag.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bl.FeatureFlag.Infrastructure.Repository;

internal class PostgreFlagContext
    : FlagContext
{
    private readonly PostgreConfig _config;

    public PostgreFlagContext(PostgreConfig config)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseNpgsql(_config.ConnectionString, cfg =>
            {
                cfg.EnableRetryOnFailure(5);
                cfg.CommandTimeout(60 * 5); // five minutes
            });
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CompleteFlagAccessModel>(b =>
        {
            b.HasKey(e => e.Id);

            b.HasIndex(e => new { e.GroupId, e.NormalizedRoleName }).IsUnique();

            b.Property(e => e.Id).ValueGeneratedOnAdd();
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
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.Name).HasMaxLength(500);
            b.Property(e => e.NormalizedName).HasMaxLength(500);
            b.Property(e => e.Description);
            b.Property(e => e.CreatedAt);
            b.Property(e => e.SubscriptionId);

            b.HasOne(e => e.UserSubscription)
                .WithMany()
                .IsRequired()
                .HasForeignKey(e => e.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<UserSubscriptionModel>(b =>
        {

        });

        builder.Entity<SubscriptionModel>(b =>
        {

        });
    }
}
