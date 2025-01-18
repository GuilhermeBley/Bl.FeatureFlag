namespace Bl.FeatureFlag.Domain.Entities.Flag;

public interface IGroupInfo
{
    Guid Id { get; }
    string Name { get; }
    string NormalizedName { get; }
}
