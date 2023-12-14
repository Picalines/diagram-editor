using DiagramEditor.Application.Extensions;
using DiagramEditor.Domain.Diagrams;

namespace DiagramEditor.Application.Repositories;

public interface IDiagramElementPropertyRepository
{
    public void AddRange(IEnumerable<DiagramElementProperty> properties);

    public void Add(DiagramElementProperty property)
    {
        AddRange(property.Yield());
    }

    public IEnumerable<DiagramElementProperty> GetAllByElement(DiagramElement element);

    public void UpdateRange(IEnumerable<DiagramElementProperty> properties);

    public void Update(DiagramElementProperty property)
    {
        UpdateRange(property.Yield());
    }

    public void RemoveAllByElement(DiagramElement element);
}
