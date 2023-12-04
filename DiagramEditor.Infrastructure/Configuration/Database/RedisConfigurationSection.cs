using Microsoft.Extensions.Configuration;

namespace DiagramEditor.Infrastructure.Configuration.Database;

public sealed class RedisConfigurationSection
{
    [ConfigurationKeyName("Redis:Host")]
    public required string Host { get; set; }

    [ConfigurationKeyName("REDIS_PORT")]
    public required int Port { get; set; }

    [ConfigurationKeyName("REDIS_PASSWORD")]
    public required string Password { get; set; }
}
