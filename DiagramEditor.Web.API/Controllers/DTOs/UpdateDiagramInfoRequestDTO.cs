using CSharpFunctionalExtensions;

namespace DiagramEditor.Web.API.Controllers.DTOs;

public sealed record UpdateDiagramInfoRequestDTO
{
    public Maybe<string> Name { get; init; }

    public Maybe<string> Description { get; init; }
}
