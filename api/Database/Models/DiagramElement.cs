namespace DiagramEditor.Database.Models;

public sealed class DiagramElement
{
    public int Id { get; set; }

    public required Diagram Diagram { get; init; }

    public required DiagramElementType Type { get; init; }

    public required float OriginX { get; set; }

    public required float OriginY { get; set; }

    public ISet<DiagramElementProperty> Properties { get; } =
        new HashSet<DiagramElementProperty>(ReferenceEqualityComparer.Instance);
}

public enum DiagramElementType
{
    Connector,
    Container,
    Display,
}
