namespace DiagramEditor.Application.UseCases.Authentication.Refresh;

public sealed record RefreshRequest : IRequest
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }
}
