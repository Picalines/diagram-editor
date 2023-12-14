namespace DiagramEditor.Domain.Diagrams;

public sealed class DiagramEnvironment
{
    public Guid Id { get; private set; }

    public required Diagram Diagram { get; init; }

    public required bool IsActive { get; set; }

    public required int ViewsCount { get; set; }
}
