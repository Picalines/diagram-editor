﻿using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Passwords;
using DiagramEditor.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UserRepository(
    ApplicationContext context,
    IPasswordHasher passwordHasher,
    IPasswordValidator passwordValidator
) : IUserRepository
{
    public Maybe<User> GetById(UserId userId)
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

        if (passwordValidator.Validate(passwordText) is false)
        {
            return UserCreationError.InvalidPassword;
        }

        var user = new User
        {
            Login = login,
            PasswordHash = passwordHasher.Hash(passwordText),
            DisplayName = login,
        };

        context.Users.Add(user);
        context.SaveChanges();

        return user;
    }
}