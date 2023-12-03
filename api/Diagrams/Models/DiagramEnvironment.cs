namespace DiagramEditor.Database.Models;

public sealed class DiagramEnvironment
{
    public int Id { get; set; }

    public required Diagram Diagram { get; init; }

    public required string PublicName { get; set; }
}
