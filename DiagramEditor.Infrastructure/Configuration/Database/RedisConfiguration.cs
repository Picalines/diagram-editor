using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Configuration.Database;

internal static class RedisConfiguration
{
    public static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var redisConfiguration =
            configuration.Get<RedisConfigurationSection>()
            ?? throw new InvalidOperationException("invalid redis configuration");

        services.AddSingleton(redisConfiguration);

        services.AddStackExchangeRedisCache(options =>
        {
            var host = $"{redisConfiguration.Host}:{redisConfiguration.Port}";
            options.Configuration = $"{host},password={redisConfiguration.Password}";
        });

        return services;
    }
}
