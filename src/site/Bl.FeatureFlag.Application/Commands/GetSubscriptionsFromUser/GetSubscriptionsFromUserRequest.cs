namespace Bl.FeatureFlag.Application.Commands.GetSubscriptionsFromUser;

public record GetSubscriptionsFromUserRequest(
    Guid UserId,
    long Skip,
    long Take)
    : IRequest<GetSubscriptionsFromUserResponse>;
