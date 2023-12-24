using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.UseCases.Diagrams.GetInfo;

public sealed record GetDiagramInfoRequest
{
    public required Guid Id { get; init; }

    public required DiagramViewMode ViewMode { get; init; }
}
