using DiagramEditor.Application.DTOs;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.GetAll;

public sealed record GetAllDiagramEnvironmentsResponse(
    IReadOnlyList<DiagramEnvironmentDTO> Environments
);

