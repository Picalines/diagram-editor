using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.DTOs;

public sealed record DiagramDTO
{
    public required Guid Id { get; init; }

    public required Guid CreatorId { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required DateTime UpdatedDate { get; init; }

    public static DiagramDTO FromDiagram(Diagram diagram) =>
        new DiagramDTO
        {
            Id = diagram.Id,
            CreatorId = diagram.Creator.Id,
            Name = diagram.Name,
            Description = diagram.Description,
            UpdatedDate = diagram.UpdatedDate,
        };
}
