namespace Bl.FeatureFlag.Application.Commands.GetSubscriptionsFromUser;

public record GetSubscriptionsFromUserRequest(
    Guid UserId,
    int Skip,
    int Take)
    : IRequest<GetSubscriptionsFromUserResponse>;
