using CSharpFunctionalExtensions;
using DiagramEditor.Application.Errors;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public enum DiagramCreationError
{
    InvalidName,
}

public enum DiagramUpdateError
{
    InvalidName,
    InvalidViewCount,
}

public sealed record DiagramUpdateDto
{
    public string? Name { get; init; }

    public string? Description { get; init; }

    public string? BannerUrl { get; init; }

    public int? ViewsCount { get; init; }
}

public interface IDiagramRepository
{
    public Maybe<Diagram> GetById(Guid id);

    public IEnumerable<Diagram> GetCreatedByUser(User user);

    public Result<Diagram, IError> Create(User creator, string name);

    public Result<Diagram, DiagramUpdateError> Update(
        Diagram diagram,
        DiagramUpdateDto updatedDiagram
    );

    public void DeleteById(Guid id);
}
