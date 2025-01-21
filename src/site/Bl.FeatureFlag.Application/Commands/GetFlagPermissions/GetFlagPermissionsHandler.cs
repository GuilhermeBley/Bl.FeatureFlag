using Bl.FeatureFlag.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.FeatureFlag.Application.Commands.GetFlagPermissions;

public class GetFlagPermissionsHandler
    : IRequestHandler<GetFlagPermissionsRequest, GetFlagPermissionsResponse>
{
    private readonly IClaimProvider _claimProvider;
    private readonly IFastFlagRepository _repository;

    public Task<GetFlagPermissionsResponse> Handle(
        GetFlagPermissionsRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
