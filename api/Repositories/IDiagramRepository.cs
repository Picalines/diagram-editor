using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Repositories;

public interface IDiagramRepository
{
    public Diagram Create(User creator, string name);

    public Maybe<Diagram> GetById(int id);

    public bool DeleteById(int id);
}
