namespace DiagramEditor.Database.Models;

public sealed class AdminNote
{
    public int Id { get; set; }

    public required User User { get; init; }

    public string Message { get; } = "";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}
