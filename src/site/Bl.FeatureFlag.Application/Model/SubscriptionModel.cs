﻿namespace Bl.FeatureFlag.Application.Model;

public class SubscriptionModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
