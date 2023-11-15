using System.Security.Claims;
using CSharpFunctionalExtensions;
using DiagramEditor.Attributes;
using DiagramEditor.Database;
using DiagramEditor.Database.Models;
using DiagramEditor.Extensions;
using DiagramEditor.Services.Authentication;
using DiagramEditor.Services.Passwords;

namespace DiagramEditor.Services;

public enum UserCreationError
{
    LoginTaken,
    InvalidPassword,
}

[Injectable(ServiceLifetime.Singleton)]
public sealed class UserService(
    ApplicationContext context,
    IAuthenticationService authentication,
    IPasswordHasher passwordHasher,
    IPasswordValidator passwordValidator
)
{
    public Maybe<User> GetById(int id)
    {
        return context.Users.SingleOrDefault(user => user.Id == id).AsMaybe();
    }

    public Maybe<User> GetByLogin(string login)
    {
        return context.Users.SingleOrDefault(user => user.Login == login).AsMaybe();
    }

    public Maybe<User> GetCurrent(ClaimsPrincipal principal)
    {
        return principal
            .Claims
            .FirstOrDefault(claim => claim.Type is "id")
            .AsMaybe()
            .Bind(idClaim => idClaim.Value.MaybeParse<int>())
            .Bind(GetById);
    }

    public Maybe<(User User, string Token)> Authenticate(string login, string passwordText)
    {
        return GetByLogin(login)
            .Where(user => passwordHasher.Verify(passwordText, user.PasswordHash))
            .Map(user => (user, authentication.GenerateToken(user)));
    }

    public Result<User, UserCreationError> CreateUser(string login, string passwordText)
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
