﻿using Bl.FeatureFlag.Application.Model;
using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Repository;

/// <summary>
/// Repository with performatic results about the flags
/// </summary>
public interface IFastFlagRepository
{
    Task AddAsync(FlagAccess flag, CancellationToken cancellationToken = default);
    Task<FlagAccessModel> GetByNameAsync(
        Guid subscriptionId,
        string groupName, 
        string roleName, 
        CancellationToken cancellationToken = default);
}
