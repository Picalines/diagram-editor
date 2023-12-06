namespace DiagramEditor.Application.UseCases.Authentication.Login;

public sealed record LoginRequest(string Login, string Password) : IRequest;
