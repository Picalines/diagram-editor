namespace DiagramEditor.Application.UseCases.Diagrams.Environments.Create;

public sealed record CreateDiagramEnvironmentRequest
{
    public required Guid DiagramId { get; init; }

    public required bool IsPublic { get; init; }
}
