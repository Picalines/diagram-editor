namespace DiagramEditor.Controllers.Requests;

public sealed record RefreshRequest
{
    public required string RefreshToken { get; set; }
}
