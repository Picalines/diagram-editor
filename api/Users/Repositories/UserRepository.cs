using CSharpFunctionalExtensions;
using DiagramEditor.Attributes;
using DiagramEditor.Database;
using DiagramEditor.Database.Models;
using DiagramEditor.Services.Passwords;

namespace DiagramEditor.Repositories;

[Injectable(ServiceLifetime.Singleton)]
public sealed class UserRepository(
    ApplicationContext context,
    IPasswordHasher passwordHasher,
    IPasswordValidator passwordValidator
) : IUserRepository
{
    public Maybe<User> GetById(int id)
    {
        return context.Users.SingleOrDefault(user => user.Id == id).AsMaybe();
    }

    public Maybe<User> GetByLogin(string login)
    {
        return context.Users.SingleOrDefault(user => user.Login == login).AsMaybe();
    }

    public Result<User, UserCreationError> Create(string login, string passwordText)
    {
        if (context.Users.Any(user => user.Login == login))
        {
            return UserCreationError.LoginTaken;
        }

        if (passwordValidator.Validate(passwordText) is false)
        {
            return UserCreationError.InvalidPassword;
        }

        var user = new User(login, passwordHasher.Hash(passwordText));

        context.Users.Add(user);
        context.SaveChanges();

        return user;
    }
}
