using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public enum UserCreationError
{
    LoginTaken,
    InvalidPassword,
}

public interface IUserRepository
{
    public Maybe<User> GetById(UserId id);

    public Maybe<User> GetByLogin(string login);

    public Result<User, UserCreationError> Register(User user);
}
