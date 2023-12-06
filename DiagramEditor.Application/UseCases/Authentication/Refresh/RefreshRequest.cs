namespace DiagramEditor.Application.UseCases.Authentication.Refresh;

public sealed record RefreshRequest(string AccessToken, string RefreshToken) : IRequest;
