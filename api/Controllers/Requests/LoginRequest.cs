namespace DiagramEditor.Controllers.Requests;

public sealed record LoginRequest
{
    public required string Login { get; set; }

    public required string Password { get; set; }
}
