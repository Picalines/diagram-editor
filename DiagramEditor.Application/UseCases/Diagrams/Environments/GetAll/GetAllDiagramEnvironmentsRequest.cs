namespace DiagramEditor.Application.UseCases.Diagrams.Environments.GetAll;

public sealed record GetAllDiagramEnvironmentsRequest
{
    public required Guid DiagramId { get; init; }
}

