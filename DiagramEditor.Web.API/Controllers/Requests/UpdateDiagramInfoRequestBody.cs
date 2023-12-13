namespace DiagramEditor.Web.API.Controllers.Requests;

public sealed record UpdateDiagramInfoRequestBody
{
    public string? Name { get; init; }

    public string? Description { get; init; }
}
