using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public sealed record DiagramUpdateDto
{
    public Maybe<string> Name { get; init; }

    public Maybe<string> Description { get; init; }
}

public interface IDiagramRepository
{
    public Maybe<Diagram> GetById(Guid id);

    public IEnumerable<Diagram> GetCreatedByUser(User user);

    public Diagram Create(User creator, string name);

    public Diagram Update(Diagram diagram, DiagramUpdateDto updatedDiagram);

    public bool DeleteById(Guid id);
}
