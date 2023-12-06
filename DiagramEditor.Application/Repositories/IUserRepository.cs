using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public enum UserCreationError
{
    InvalidLogin,
    InvalidPassword,
    LoginTaken,
}

public interface IUserRepository
{
    public Maybe<User> GetById(Guid id);

    public Maybe<User> GetByLogin(string login);

    public Result<User, UserCreationError> Register(string login, string passwordText);
}
