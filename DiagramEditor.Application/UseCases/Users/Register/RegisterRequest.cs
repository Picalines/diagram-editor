namespace DiagramEditor.Application.UseCases.Users.Register;

public sealed record RegisterRequest
{
    public required string Login { get; set; }

    public required string Password { get; set; }
}
