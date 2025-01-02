namespace Bl.FeatureFlag.Domain.Entities;

public class Entity
    : IEntity
{
    public virtual Guid EntityId { get; } = Guid.NewGuid();
}

public interface IEntity
{
    public Guid EntityId { get; }
}
