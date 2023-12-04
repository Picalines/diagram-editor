using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Domain;

public sealed class DiagramEnvironment
{
    public Guid Id { get; private set; }

    public required Diagram Diagram { get; init; }

    public required string PublicUrl { get; set; }

    public required string PublicName { get; set; }
}
