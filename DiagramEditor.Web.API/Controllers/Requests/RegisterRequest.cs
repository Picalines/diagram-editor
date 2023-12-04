namespace DiagramEditor.Web.API.Controllers.Requests;

public sealed record RegisterRequest
{
    public required string Login { get; set; }

    public required string Password { get; set; }
}
