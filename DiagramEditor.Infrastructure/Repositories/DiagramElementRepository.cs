using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DiagramElementRepository(ApplicationContext context)
    : IDiagramElementRepository
{
    public void Add(DiagramElement element)
    {
        context.DiagramElements.Attach(element);
        context.SaveChanges();
    }

    public Maybe<DiagramElement> GetById(Guid id)
    {
        return context
            .DiagramElements.Include(e => e.Diagram)
            .ThenInclude(d => d.User)
            .SingleOrDefault(element => element.Id == id)
            .AsMaybe();
    }

    public IEnumerable<DiagramElement> GetAllByDiagram(Diagram diagram)
    {
        return context
            .DiagramElements.Include(e => e.Diagram)
            .ThenInclude(d => d.User)
            .Where(element => element.Diagram.Id == diagram.Id);
    }

    public void Update(DiagramElement element)
    {
        context.DiagramElements.Update(element);
        context.SaveChanges();
    }

    public void Remove(DiagramElement element)
    {
        context.DiagramElements.Remove(element);
        context.SaveChanges();
    }

    public void RemoveAllByDiagram(Diagram diagram)
    {
        // TODO: bulk delete
        context.DiagramElements.RemoveRange(GetAllByDiagram(diagram));
        context.SaveChanges();
    }
}
