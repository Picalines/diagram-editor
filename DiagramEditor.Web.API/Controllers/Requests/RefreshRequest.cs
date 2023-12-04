namespace DiagramEditor.Web.API.Controllers.Requests;

public sealed record RefreshRequest
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}
