namespace Bl.FeatureFlag.Application.Repository;

public abstract class FlagContext
    : DbContext
{
    public DbSet<> MyProperty { get; set; }
}
