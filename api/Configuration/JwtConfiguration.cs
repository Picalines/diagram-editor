namespace DiagramEditor.Configuration;

public sealed class JwtConfiguration
{
    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required JwtTokenConfiguration AccessToken { get; set; }

    public required JwtTokenConfiguration RefreshToken { get; set; }
}

public sealed class JwtTokenConfiguration
{
    public required string Secret { get; set; }

    public required int ExpirationMinutes { get; set; }
}
