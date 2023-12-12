namespace DiagramEditor.Application.UseCases.Diagrams.Delete;

public sealed record DeleteDiagramRequest
{
    public required Guid Id { get; init; }
}
