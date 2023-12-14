using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DiagramEnvironmentRepository(ApplicationContext context)
    : IDiagramEnvironmentRepository
{
    public void Add(DiagramEnvironment environment)
    {
        context.DiagramEnvironments.Add(environment);
        context.SaveChanges();
    }

    public Maybe<DiagramEnvironment> GetById(Guid id)
    {
        return context.DiagramEnvironments.SingleOrDefault(env => env.Id == id).AsMaybe();
    }

    public IEnumerable<DiagramEnvironment> GetAllByDiagram(Diagram diagram)
    {
        return context.DiagramEnvironments.Where(env => env.Diagram.Id == diagram.Id);
    }

    public void Update(DiagramEnvironment environment)
    {
        context.DiagramEnvironments.Update(environment);
        context.SaveChanges();
    }

    public void Remove(DiagramEnvironment environment)
    {
        context.Remove(environment);
        context.SaveChanges();
    }

    public void RemoveAllByDiagram(Diagram diagram)
    {
        // TODO: bulk remove
        context.RemoveRange(GetAllByDiagram(diagram));
        context.SaveChanges();
    }
}