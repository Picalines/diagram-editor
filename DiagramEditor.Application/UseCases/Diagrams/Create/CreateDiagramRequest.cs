using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.UseCases.Diagrams.Create;

public sealed record CreateDiagramRequest
{
    public required string Name { get; init; }

    public Maybe<string> Description { get; init; }
}
