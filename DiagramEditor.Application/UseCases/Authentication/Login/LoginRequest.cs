namespace DiagramEditor.Application.UseCases.Authentication.Login;

public sealed record LoginRequest
{
    public required string Login { get; init; }

    public required string Password { get; init; }
}
