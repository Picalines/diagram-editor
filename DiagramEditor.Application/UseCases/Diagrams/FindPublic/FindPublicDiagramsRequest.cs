namespace DiagramEditor.Application.UseCases.Diagrams.FindPublic;

public sealed record FindPublicDiagramsRequest
{
    public required string Query { get; init; }
}

