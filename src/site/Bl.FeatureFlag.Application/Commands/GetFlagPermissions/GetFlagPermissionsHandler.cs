using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.FeatureFlag.Application.Commands.GetFlagPermissions;

public class GetFlagPermissionsHandler
    : IRequestHandler<GetFlagPermissionsRequest, GetFlagPermissionsResponse>
{
    public Task<GetFlagPermissionsResponse> Handle(
        GetFlagPermissionsRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
