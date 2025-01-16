﻿namespace Bl.FeatureFlag.Application.Commands.CreateFlag;

public record CreateFlagItem(
    string Name,
    string? Description,
    DateTime? ExpiresAt);

public record CreateFlagRequest(
    Guid SubscriptionId,
    string GroupName,
    string? GroupDescription,
    CreateFlagItem[] Items)
    : IRequest<CreateFlagResponse>;
