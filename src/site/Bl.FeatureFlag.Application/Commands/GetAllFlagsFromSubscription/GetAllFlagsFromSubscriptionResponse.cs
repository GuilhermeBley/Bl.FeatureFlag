namespace Bl.FeatureFlag.Application.Commands.GetAllFlagsFromSubscription;

public record GetAllFlagsFromSubscriptionResponse(
    GetAllFlagsFromSubscriptionSubInfo[] Infos);

/// <summary>
/// Subscription info with all flags
/// </summary>
public class GetAllFlagsFromSubscriptionSubInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public List<GetAllFlagsFromSubscriptionFlagInfo> Flags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}


public record GetAllFlagsFromSubscriptionFlagInfo
{
    public Guid FlagId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string NormalizedGroupName { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime FlagCreatedAt { get; set; }
}
