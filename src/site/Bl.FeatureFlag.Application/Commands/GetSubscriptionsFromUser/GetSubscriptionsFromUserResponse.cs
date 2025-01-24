namespace Bl.FeatureFlag.Application.Commands.GetSubscriptionsFromUser;

public record GetSubscriptionsFromUserItem();

public record GetSubscriptionsFromUserResponse(
    Guid UserId,
    IReadOnlyList<GetSubscriptionsFromUserItem> Items);
