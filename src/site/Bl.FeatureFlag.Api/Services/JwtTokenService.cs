using System.Security.Claims;

namespace Bl.FeatureFlag.Api.Services;

public record TokenResult(
    string Token);

public interface IJwtTokenService
{
    Task<TokenResult> GenerateTokenAsync(
        Claim[] claims,
        TimeSpan? expiresIn,
        CancellationToken cancellationToken = default);
}

public class JwtTokenService
    : IJwtTokenService
{

}
