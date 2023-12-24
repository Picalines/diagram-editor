using DiagramEditor.Application.DTOs;

namespace DiagramEditor.Application.UseCases.Diagrams.FindPublic;

public sealed record FindPublicDiagramsResponse(IReadOnlyList<DiagramDTO> Diagrams);

