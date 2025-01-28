﻿using Bl.FeatureFlag.Application.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bl.FeatureFlag.Application.Repository;

public abstract class FlagContext
    : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public DbSet<CompleteFlagAccessModel> Flags { get; set; }
    public DbSet<FlagGroupModel> FlagGroups { get; set; }
    public DbSet<UserSubscriptionModel> UserSubscriptions { get; set; }
    public DbSet<SubscriptionModel> Subscriptions { get; set; }
}
