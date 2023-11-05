namespace DiagramEditor.Database.Models;

public sealed class DiagramElementProperty
{
    public int Id { get; set; }

    public required string Key { get; set; }

    public required string Value { get; set; }
}
