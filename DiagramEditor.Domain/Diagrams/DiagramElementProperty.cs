namespace DiagramEditor.Domain.Diagrams;

public sealed class DiagramElementProperty
{
    public Guid Id { get; private set; }

    public required DiagramElement Element { get; init; }

    public required string Key { get; set; }

    public required string Value { get; set; }
}
