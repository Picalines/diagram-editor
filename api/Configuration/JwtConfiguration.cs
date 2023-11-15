namespace DiagramEditor.Configuration;

public sealed class JwtConfiguration
{
    public required string SignInSecret { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }
}
