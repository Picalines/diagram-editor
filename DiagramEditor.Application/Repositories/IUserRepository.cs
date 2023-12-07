using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.Repositories;

public enum UserCreationError
{
    InvalidLogin,
    InvalidPassword,
    LoginTaken,
}

public enum UserUpdateError
{
    InvalidLogin,
    InvalidPassword,
    LoginTaken,
}

public sealed record UpdateUserDto
{
    public string? Login { get; init; }

    public string? Password { get; init; }

    public string? DisplayName { get; init; }

    public string? AvatarUrl { get; init; }
}

public interface IUserRepository
{
    public Maybe<User> GetById(Guid id);

    public Maybe<User> GetByLogin(string login);

    public Result<User, UserCreationError> Register(string login, string passwordText);

    public Result<User, UserUpdateError> Update(User user, UpdateUserDto updatedUser);
}
