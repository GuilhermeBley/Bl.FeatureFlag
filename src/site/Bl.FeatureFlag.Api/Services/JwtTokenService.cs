using Bl.FeatureFlag.Api.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
    private readonly static TimeSpan _defaultExpiration = TimeSpan.FromMinutes(15);

    private readonly IOptions<JwtConfig> _config;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(
        IOptions<JwtConfig> config,
        ILogger<JwtTokenService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public Task<TokenResult> GenerateTokenAsync(
        Claim[] claims, 
        TimeSpan? expiresIn, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var key = _config.Value.Key;
            var issuer = _config.Value.Issuer;
            var audience = _config.Value.Audience;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expiration = expiresIn ?? _defaultExpiration;
            var tokenExpiration = DateTime.UtcNow.Add(expiration);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            var result = new TokenResult(tokenString);

            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate JWT token");
            throw;
        }
    }
}
