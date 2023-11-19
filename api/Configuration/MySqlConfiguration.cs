namespace DiagramEditor.Configuration;

public sealed class MySqlConfiguration
{
    [ConfigurationKeyName("MySql:Server")]
    public required string Server { get; set; }

    [ConfigurationKeyName("MySql:Version")]
    public required string Version { get; set; }

    [ConfigurationKeyName("MYSQL_ROOT_PASSWORD")]
    public required string RootPassword { get; set; }

    [ConfigurationKeyName("MYSQL_DATABASE")]
    public required string Database { get; set; }
}
