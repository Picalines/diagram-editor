using DiagramEditor.Application.Extensions;
using DiagramEditor.Infrastructure.Configuration.Authentication;
using DiagramEditor.Infrastructure.Configuration.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        MySqlConfiguration.AddMySql(services, configuration);
        RedisConfiguration.AddRedis(services, configuration);

        JwtConfiguration.AddJwtBearerServices(services, configuration);

        services.AddInjectableFromCallingAssembly();

        return services;
    }
}
