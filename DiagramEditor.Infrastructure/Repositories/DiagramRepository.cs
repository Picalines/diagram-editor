using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DiagramRepository(ApplicationContext context) : IDiagramRepository
{
    public void Add(Diagram diagram)
    {
        context.Diagrams.Attach(diagram);
        context.SaveChanges();
    }

    public Maybe<Diagram> GetById(Guid id)
    {
        return context
            .Diagrams.Include(d => d.User)
            .SingleOrDefault(diagram => diagram.Id == id)
            .AsMaybe();
    }

    public IEnumerable<Diagram> GetCreatedByUser(User user)
    {
        return context.Diagrams.Include(d => d.User).Where(diagram => diagram.User.Id == user.Id);
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
