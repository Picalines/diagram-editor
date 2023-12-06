namespace DiagramEditor.Application.UseCases.Authentication.Login;

public sealed record LoginResponse(string AccessToken, string RefreshToken) : IResponse;
