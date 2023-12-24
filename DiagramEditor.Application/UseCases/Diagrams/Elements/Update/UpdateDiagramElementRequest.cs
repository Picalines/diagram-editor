using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.UseCase;

public sealed record UpdateDiagramElementRequest
{
    public required Guid Id { get; init; }

    public required Maybe<float> OriginX { get; init; }

    public required Maybe<float> OriginY { get; init; }

    public required Maybe<IReadOnlyDictionary<string, string>> Properties { get; init; }
}
