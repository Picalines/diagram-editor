namespace DiagramEditor.Configuration;

public sealed class RedisConfiguration
{
    [ConfigurationKeyName("Redis:Host")]
    public required string Host { get; set; }

    [ConfigurationKeyName("REDIS_PORT")]
    public required int Port { get; set; }

    [ConfigurationKeyName("REDIS_PASSWORD")]
    public required string Password { get; set; }
}
