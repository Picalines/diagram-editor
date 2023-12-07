namespace DiagramEditor.Domain.Diagrams;

public sealed class DiagramElement
{
    public Guid Id { get; private set; }

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
