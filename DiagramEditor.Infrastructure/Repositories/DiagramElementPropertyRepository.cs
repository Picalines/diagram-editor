using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DiagramElementPropertyRepository(ApplicationContext context)
    : IDiagramElementPropertyRepository
{
    public void AddRange(IEnumerable<DiagramElementProperty> properties)
    {
        context.DiagramElementProperties.AttachRange(properties);
        context.SaveChanges();
    }

    public IEnumerable<DiagramElementProperty> GetAllByElement(DiagramElement element)
    {
        return context.DiagramElementProperties.Where(
            property => property.Element.Id == element.Id
        );
    }

    public void UpdateRange(IEnumerable<DiagramElementProperty> properties)
    {
        context.UpdateRange(properties);
        context.SaveChanges();
    }

    public void RemoveAllByElement(DiagramElement element)
    {
        // TODO: bulk delete
        context.DiagramElementProperties.RemoveRange(GetAllByElement(element));
        context.SaveChanges();
    }
}
