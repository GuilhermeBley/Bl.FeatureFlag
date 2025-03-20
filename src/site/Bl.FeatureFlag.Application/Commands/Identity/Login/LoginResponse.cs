using System.Security.Claims;

namespace Bl.FeatureFlag.Application.Commands.Identity.Login;

public record LoginResponse(
    Guid UserId,
    Claim[] Claims);
