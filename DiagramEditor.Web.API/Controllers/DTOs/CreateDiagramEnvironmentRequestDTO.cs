namespace DiagramEditor.Web.API.Controllers.DTOs;

public sealed record CreateDiagramEnvironmentRequestDTO
{
    public required bool IsPublic { get; init; }
}
