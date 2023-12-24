using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.Update;

public sealed record UpdateDiagramEnvironmentRequest
{
    public required Guid Id { get; init; }

    public Maybe<bool> IsActive { get; init; }

    public Maybe<int> ViewsCount { get; init; }
}
