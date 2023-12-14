using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DiagramRepository(ApplicationContext context) : IDiagramRepository
{
    public void Add(Diagram diagram)
    {
        context.Diagrams.Add(diagram);
        context.SaveChanges();
    }

    public Maybe<Diagram> GetById(Guid id)
    {
        return context.Diagrams.SingleOrDefault(diagram => diagram.Id == id).AsMaybe();
    }

    public IEnumerable<Diagram> GetCreatedByUser(User user)
    {
        return context.Diagrams.Where(diagram => diagram.Creator.Id == user.Id);
    }

    public void Update(Diagram diagram)
    {
        context.Diagrams.Update(diagram);
        context.SaveChanges();
    }

    public void Remove(Diagram diagram)
    {
        context.Diagrams.Remove(diagram);
        context.SaveChanges();
    }
}
