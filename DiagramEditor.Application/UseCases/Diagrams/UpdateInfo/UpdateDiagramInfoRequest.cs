using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.UseCases.Diagrams.UpdateInfo;

public sealed record UpdateDiagramInfoRequest
{
    public required Guid Id { get; init; }

    public Maybe<string> Name { get; init; }

    public Maybe<string> Description { get; init; }
}
