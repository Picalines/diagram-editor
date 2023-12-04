namespace DiagramEditor.Domain.Diagrams;

public readonly record struct DiagramElementId(Guid Value);

public sealed class DiagramElement
{
    public DiagramElementId Id { get; private set; }

    public required Diagram Diagram { get; init; }

    public required DiagramElementType Type { get; init; }

    public required float OriginX { get; set; }

    public required float OriginY { get; set; }
}

public enum DiagramElementType
{
    Connector,
    Container,
    Display,
}