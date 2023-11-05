namespace DiagramEditor.Database.Models;

public sealed class Diagram
{
    public int Id { get; set; }

    public required User Creator { get; init; }

    public required string Name { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public int ViewsCount { get; set; } = 0;

    public ISet<DiagramEnvironment> Environments { get; } =
        new HashSet<DiagramEnvironment>(ReferenceEqualityComparer.Instance);

    public ISet<DiagramAccess> Accesses { get; } =
        new HashSet<DiagramAccess>(ReferenceEqualityComparer.Instance);

    public ISet<DiagramElement> Elements { get; } =
        new HashSet<DiagramElement>(ReferenceEqualityComparer.Instance);
}
