namespace DiagramEditor.Database.Models;

public sealed class DiagramAccess
{
    public int Id { get; set; }

    public required Diagram Diagram { get; init; }

    public required User User { get; init; }

    public required bool AllowEdit { get; set; }
}
