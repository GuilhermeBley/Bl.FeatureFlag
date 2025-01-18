using Bl.FeatureFlag.Domain.Entities.Flag;

namespace Bl.FeatureFlag.Application.Repository;

/// <summary>
/// Repository with performatic results about the flags
/// </summary>
public interface IFastFlagRepository
{
    Task AddAsync(FlagAccess flag, CancellationToken cancellationToken = default);
}
