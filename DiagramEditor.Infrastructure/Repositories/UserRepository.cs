using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Passwords;
using DiagramEditor.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UserRepository(ApplicationContext context, IPasswordHasher passwordHasher)
    : IUserRepository
{
    public Maybe<User> GetById(Guid userId)
    {
        return context.Users.SingleOrDefault(user => user.Id == userId).AsMaybe();
    }

    public Maybe<User> GetByLogin(string login)
    {
        return context.Users.SingleOrDefault(user => user.Login == login).AsMaybe();
    }

    public Result<User, UserCreationError> Register(string login, string passwordText)
    {
        if (context.Users.Any(user => user.Login == login))
        {
            return UserCreationError.LoginTaken;
        }

        var user = new User(login, passwordHasher.Hash(passwordText));

        context.Users.Add(user);
        context.SaveChanges();

        return user;
    }

    public Result<User, UserUpdateError> Update(User user, UpdateUserDto updatedUser)
    {
        if (updatedUser.Login.TryGetValue(out var login) is true)
        {
            if (context.Users.Any(user => user.Login == login))
            {
                return UserUpdateError.LoginTaken;
            }

            user.Login = login;
        }

        updatedUser.Password.Execute(password => user.PasswordHash = passwordHasher.Hash(password));
        updatedUser.DisplayName.Execute(displayName => user.DisplayName = displayName);
        updatedUser.AvatarUrl.Execute(avatarUrl => user.AvatarUrl = avatarUrl);

        context.SaveChanges();

        return user;
    }
}
