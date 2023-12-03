namespace DiagramEditor.Controllers.Responses;

public sealed record ErrorMessageResponse(string message)
{
    public string Error { get; set; } = message;
}
