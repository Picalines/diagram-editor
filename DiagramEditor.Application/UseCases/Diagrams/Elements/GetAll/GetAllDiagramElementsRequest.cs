using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;

public sealed record GetAllDiagramElementsRequest
{
    public required Guid Id { get; init; }

    public required DiagramViewMode ViewMode { get; init; }
}
