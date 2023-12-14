using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public interface IDiagramRepository
{
    public void Add(Diagram diagram);

    public Maybe<Diagram> GetById(Guid id);

    public IEnumerable<Diagram> GetCreatedByUser(User user);

    public void Update(Diagram diagram);

    public void Remove(Diagram diagram);
}
