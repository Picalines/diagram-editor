namespace DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;

public enum DiagramViewMode
{
    InEditor,
    FromEnvironment,
}

public sealed record GetAllDiagramElementsRequest
{
    public required Guid Id { get; init; }

    public required DiagramViewMode ViewMode { get; init; }
}
