using CSharpFunctionalExtensions;

namespace DiagramEditor.Web.API.Controllers.DTOs;

public sealed record UpdateDiagramEnvironmentRequestDTO
{
    public Maybe<bool> IsActive { get; init; }

    public Maybe<int> ViewsCount { get; init; }
}
