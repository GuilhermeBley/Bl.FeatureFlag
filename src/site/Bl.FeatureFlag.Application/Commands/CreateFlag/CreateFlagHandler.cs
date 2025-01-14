using Bl.FeatureFlag.Application.Repository;
using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Commands.CreateFlag;

public class CreateFlagHandler
    : IRequestHandler<CreateFlagRequest, CreateFlagResponse>
{
    private readonly FlagContext _context;

    public async Task<CreateFlagResponse> Handle(
        CreateFlagRequest request, 
        CancellationToken cancellationToken)
    {
        var flag = CompleteFlagAccess.Create(
            roleName: request.Name,
            description: request.Description ?? string.Empty,
            obs: string.Empty,
            active: true,
            expiresAt: request.ExpiresAt,
            createdAt: DateTime.UtcNow)
            .RequiredResult;

        var flagAlreadyRegistered =
            await _context.Flags
            .AsNoTracking()
            .AnyAsync(e => e.NormalizedRoleName == flag.NormalizedRoleName, cancellationToken);

        if (flagAlreadyRegistered) throw new CoreException(CoreExceptionCode.Conflict);


    }
}
