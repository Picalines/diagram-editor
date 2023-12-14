using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.Repositories;

public interface IDiagramElementRepository
{
    public void Add(DiagramElement element);

    public Maybe<DiagramElement> GetById(Guid id);

    public IEnumerable<DiagramElement> GetAllByDiagram(Diagram diagram);

    public void Update(DiagramElement element);

    public void Remove(DiagramElement element);

    public void RemoveAllByDiagram(Diagram diagram);
}
