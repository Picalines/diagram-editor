using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.Repositories;

public interface IDiagramEnvironmentRepository
{
    public void Add(DiagramEnvironment environment);

    public Maybe<DiagramEnvironment> GetById(Guid id);

    public IEnumerable<DiagramEnvironment> GetAllByDiagram(Diagram diagram);

    public void Update(DiagramEnvironment environment);

    public void Remove(DiagramEnvironment environment);

    public void RemoveAllByDiagram(Diagram diagram);
}
