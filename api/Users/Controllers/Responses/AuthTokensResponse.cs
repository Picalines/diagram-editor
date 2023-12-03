namespace DiagramEditor.Controllers.Responses;

public sealed record AuthTokensResponse
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public static AuthTokensResponse FromTuple((string AccessToken, string RefreshToken) tokens)
    {
        return new() { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken };
    }
}
