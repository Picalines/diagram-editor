using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.DTOs;

public sealed record DiagramEnvironmentDTO
{
    public required Guid Id { get; init; }

    public required Guid DiagramId { get; init; }

    public required bool IsPublic { get; init; }

    public required bool IsActive { get; init; }

    public required int ViewsCount { get; init; }

    public static DiagramEnvironmentDTO FromEnvironment(DiagramEnvironment environment) =>
        new()
        {
            Id = environment.Id,
            DiagramId = environment.Diagram.Id,
            IsPublic = environment.IsPublic,
            IsActive = environment.IsActive,
            ViewsCount = environment.ViewsCount,
        };
}
