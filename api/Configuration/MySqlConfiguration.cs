namespace DiagramEditor.Configuration;

public sealed class MySqlConfiguration
{
    public required string Server { get; set; }

    public required string User { get; set; }

    public required string Password { get; set; }

    public required string Database { get; set; }

    public required string Version { get; set; }
}
