namespace DiagramEditor.Application.UseCases.Authentication.Refresh;

public sealed record RefreshResponse(string AccessToken, string RefreshToken) : IResponse;
