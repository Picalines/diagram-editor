namespace DiagramEditor.Domain.Diagrams;

public readonly record struct DiagramElementPropertyId(Guid Value);

public sealed class DiagramElementProperty
{
    public DiagramElementPropertyId Id { get; private set; }

    public required DiagramElement Element { get; init; }

    public required string Key { get; set; }

    public required string Value { get; set; }
}
