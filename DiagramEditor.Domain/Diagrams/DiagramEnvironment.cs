using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Domain;

public readonly record struct DiagramEnvironmentId(Guid Value);

public sealed class DiagramEnvironment
{
    public DiagramEnvironmentId Id { get; private set; }

    public required Diagram Diagram { get; init; }

    public required string PublicUrl { get; set; }

    public required string PublicName { get; set; }
}
