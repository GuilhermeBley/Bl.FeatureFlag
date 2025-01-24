
namespace Bl.FeatureFlag.Application.Commands.GetSubscriptionsFromUser;

public class GetSubscriptionsFromUserHandler
    : IRequestHandler<GetSubscriptionsFromUserRequest, GetSubscriptionsFromUserResponse>
{
    public Task<GetSubscriptionsFromUserResponse> Handle(
        GetSubscriptionsFromUserRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
