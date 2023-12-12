using DiagramEditor.Domain.Users;

namespace DiagramEditor.Domain.Diagrams;

public sealed class Diagram
{
    public Guid Id { get; private set; }

    public required User Creator { get; init; }

    public required string Name { get; set; }

    public required string Description { get; set; } = "";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}
