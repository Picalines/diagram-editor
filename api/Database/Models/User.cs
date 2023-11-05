namespace DiagramEditor.Database.Models;

public sealed class User
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public string? AvatarUrl { get; set; } = null;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsAdmin { get; set; } = false;

    public ISet<Diagram> Diagrams { get; } =
        new HashSet<Diagram>(ReferenceEqualityComparer.Instance);

    public ISet<DiagramAccess> Accesses { get; } =
        new HashSet<DiagramAccess>(ReferenceEqualityComparer.Instance);

    public ISet<AdminNote> AdminNotes { get; } =
        new HashSet<AdminNote>(ReferenceEqualityComparer.Instance);
}
