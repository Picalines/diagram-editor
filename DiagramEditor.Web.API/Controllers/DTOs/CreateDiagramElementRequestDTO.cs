using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Web.API.Controllers.DTOs;

public sealed record CreateDiagramElementRequestDTO
{
    public required DiagramElementType Type { get; init; }

    public required float OriginX { get; init; }

    public required float OriginY { get; init; }

    public required IReadOnlyDictionary<string, string> Properties { get; init; }
}
