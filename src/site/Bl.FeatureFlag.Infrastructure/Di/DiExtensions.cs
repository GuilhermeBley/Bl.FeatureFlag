using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bl.FeatureFlag.Infrastructure.Di;

public static class DiExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<Configuration.PostgreConfig>()
            .Configure(c => configuration.GetSection(Configuration.PostgreConfig.SECTION))
            .ValidateOnStart();

        return services
            .AddDbContext<Application.Repository.FlagContext, Repository.PostgreFlagContext>();
    }
}
