using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.DTOs;

public sealed record DiagramElementDTO
{
    public required Guid Id { get; init; }

    public required Guid DiagramId { get; init; }

    public required DiagramElementType Type { get; init; }

    public required float OriginX { get; init; }

    public required float OriginY { get; init; }

    public required IReadOnlyDictionary<string, string> Properties { get; init; }

    public static DiagramElementDTO FromElement(
        DiagramElement element,
        IReadOnlyDictionary<string, string> properties
    ) =>
        new DiagramElementDTO
        {
            Id = element.Id,
            DiagramId = element.Diagram.Id,
            Type = element.Type,
            OriginX = element.OriginX,
            OriginY = element.OriginY,
            Properties = properties,
        };

    public static DiagramElementDTO FromElement(
        DiagramElement element,
        IDiagramElementPropertyRepository properties
    ) =>
        FromElement(element, properties.GetAllByElement(element).ToDictionary(e => e.Key, e => e.Value));
}
