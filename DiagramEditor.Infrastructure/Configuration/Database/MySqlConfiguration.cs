using DiagramEditor.Infrastructure.Configuration.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure;

internal static class MySqlConfiguration
{
    public static IServiceCollection AddMySql(this IServiceCollection services, IConfiguration configuration)
    {
        var mySqlConfiguration = configuration.Get<MySqlConfigurationSection>()
            ?? throw new InvalidOperationException("invalid MySql configuration");

        services.AddSingleton(mySqlConfiguration);

        return services;
    }
}
