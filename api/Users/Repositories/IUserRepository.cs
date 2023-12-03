using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Repositories;

public enum UserCreationError
{
    LoginTaken,
    InvalidPassword,
}

public interface IUserRepository
{
    public Maybe<User> GetById(int id);

    public Maybe<User> GetByLogin(string login);

    public Result<User, UserCreationError> Create(string login, string passwordText);
}
