using DiagramEditor.Application.DTOs;

namespace DiagramEditor.Application.UseCases.Diagrams.GetOwned;

public sealed record GetOwnedDiagramsResponse(IReadOnlyList<DiagramDTO> Diagrams);
