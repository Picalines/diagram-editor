namespace DiagramEditor.Configuration;

public static class DistributedCacheConfiguration
{
    public static void UseDistributedCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var redisConfiguration =
            configuration.GetSection("Redis").Get<RedisConfiguration>()
            ?? throw new InvalidOperationException("invalid redis configuration");

        services.AddSingleton(redisConfiguration);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"redis:{redisConfiguration}";
        });
    }
}
