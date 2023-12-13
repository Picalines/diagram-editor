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
    public Diagram Create(User creator, string name)
    {
        var diagram = new Diagram
        {
            Creator = creator,
            Name = name,
            Description = ""
        };

        context.Diagrams.Add(diagram);
        context.SaveChanges();

        return diagram;
    }

    public Maybe<Diagram> GetById(Guid id)
    {
        return context.Diagrams.SingleOrDefault(diagram => diagram.Id == id).AsMaybe();
    }

    public IEnumerable<Diagram> GetCreatedByUser(User user)
    {
        return context.Diagrams.Where(diagram => diagram.Creator.Id == user.Id);
    }

    public Diagram Update(Diagram diagram, DiagramUpdateDto updatedDiagram)
    {
        updatedDiagram.Name.Execute(name => diagram.Name = name);
        updatedDiagram.Description.Execute(description => diagram.Description = description);

        context.SaveChanges();

        return diagram;
    }

    public bool DeleteById(Guid id)
    {
        return GetById(id).Map(context.Diagrams.Remove).HasValue;
    }
}
