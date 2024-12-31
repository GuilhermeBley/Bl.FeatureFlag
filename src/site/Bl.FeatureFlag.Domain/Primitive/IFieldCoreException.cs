namespace Bl.FeatureFlag.Domain.Primitive;

public interface IFieldCoreException
    : ICoreException
{
    public Type ClassType { get; }
    public Type PropertyType { get; }
    string FieldName { get; }
}
