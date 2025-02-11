using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bl.FeatureFlag.Infrastructure.Di;

public static class DiExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddDbContext<Application.Repository.FlagContext, Repository.PostgreFlagContext>();
    }
}
