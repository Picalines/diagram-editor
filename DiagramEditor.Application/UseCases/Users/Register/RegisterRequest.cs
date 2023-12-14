namespace DiagramEditor.Application.UseCases.Users.Register;

public sealed record RegisterRequest
{
    public required string Login { get; init; }

    public required string Password { get; init; }
}
