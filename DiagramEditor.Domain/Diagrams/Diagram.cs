using DiagramEditor.Domain.Users;

namespace DiagramEditor.Domain.Diagrams;

public readonly record struct DiagramId(Guid Value);

public sealed class Diagram
{
    public DiagramId Id { get; private set; }

    public required User Creator { get; init; }

    public required string Name { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public int ViewsCount { get; set; } = 0;
}
