using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.UseCases.Diagrams.GetOwned;

public sealed record GetOwnedDiagramsResponse(IReadOnlyList<Diagram> Diagrams);
