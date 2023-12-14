using DiagramEditor.Application.DTOs;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;

public sealed record GetAllDiagramElementsResponse(IReadOnlyList<DiagramElementDTO> Elements);
