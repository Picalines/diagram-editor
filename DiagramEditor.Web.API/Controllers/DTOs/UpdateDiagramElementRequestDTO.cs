using CSharpFunctionalExtensions;

namespace DiagramEditor.Web.API.Controllers.DTOs;

public sealed record UpdateDiagramElementRequestDTO
{
    public required Maybe<float> OriginX { get; init; }

    public required Maybe<float> OriginY { get; init; }

    public required Maybe<IReadOnlyDictionary<string, string>> Properties { get; init; }
}
