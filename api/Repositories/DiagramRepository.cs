using CSharpFunctionalExtensions;
using DiagramEditor.Attributes;
using DiagramEditor.Database;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Repositories;

[Injectable(ServiceLifetime.Singleton)]
public sealed class DiagramRepository(ApplicationContext context) : IDiagramRepository
{
    public Diagram Create(User creator, string name)
    {
        var diagram = new Diagram() {Creator = creator, Name = name};
        context.Diagrams.Add(diagram);
        return diagram;
    }

    public Maybe<Diagram> GetById(int id)
    {
        return context.Diagrams.SingleOrDefault(diagram => diagram.Id == id).AsMaybe();
    }

    public bool DeleteById(int id)
    {
        return GetById(id)
            .Map(context.Diagrams.Remove)
            .Map(_ => true)
            .GetValueOrDefault(false);
    }
}
