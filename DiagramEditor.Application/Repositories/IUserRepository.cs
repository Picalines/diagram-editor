using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public interface IUserRepository
{
    public void Add(User user);

    public Maybe<User> GetById(Guid id);

    public Maybe<User> GetByLogin(string login);

    public void Update(User user);
}
