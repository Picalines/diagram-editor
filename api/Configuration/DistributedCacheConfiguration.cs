namespace DiagramEditor.Configuration;

public static class DistributedCacheConfiguration
{
    public static void UseDistributedCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var redisConfiguration =
            configuration.Get<RedisConfiguration>()
            ?? throw new InvalidOperationException("invalid redis configuration");

        services.AddSingleton(redisConfiguration);

        services.AddStackExchangeRedisCache(options =>
        {
            var host = $"{redisConfiguration.Host}:{redisConfiguration.Port}";
            options.Configuration = $"{host},password={redisConfiguration.Password}";
        });
    }
}
