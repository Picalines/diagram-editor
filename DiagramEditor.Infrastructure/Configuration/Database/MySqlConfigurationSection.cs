using Microsoft.Extensions.Configuration;

namespace DiagramEditor.Infrastructure.Configuration.Database;

public sealed class MySqlConfigurationSection
{
    [ConfigurationKeyName("MYSQL_SERVER")]
    public required string Server { get; set; }

    [ConfigurationKeyName("MySql:Version")]
    public required string Version { get; set; }

    [ConfigurationKeyName("MYSQL_ROOT_PASSWORD")]
    public required string RootPassword { get; set; }

    [ConfigurationKeyName("MYSQL_DATABASE")]
    public required string Database { get; set; }
}
