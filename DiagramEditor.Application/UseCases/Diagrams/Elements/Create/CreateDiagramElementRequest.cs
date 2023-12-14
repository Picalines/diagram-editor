using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.Create;

public sealed record CreateDiagramElementRequest
{
    public required Guid DiagramId { get; init; }

    public required DiagramElementType Type { get; init; }

    public required float OriginX { get; init; }

    public required float OriginY { get; init; }
}

