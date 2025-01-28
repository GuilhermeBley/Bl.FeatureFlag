namespace Bl.FeatureFlag.Application.Commands.GetSubscriptionsFromUser;

public record GetSubscriptionsFromUserItem(
    Guid Id,
    string Name,
    string NormalizedName,
    DateTime CreatedAt);

public record GetSubscriptionsFromUserResponse(
    Guid UserId,
    GetSubscriptionsFromUserItem[] Items);
