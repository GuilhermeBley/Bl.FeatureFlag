namespace Bl.FeatureFlag.Application.Commands.CreateFlag;

public record CreateFlagRequest(
    string Name,
    string? Description,
    DateTime? ExpiresAt)
    : IRequest<CreateFlagResponse>;
