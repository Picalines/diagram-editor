using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Services.Authentication;

public interface IAuthenticator
{
    public Maybe<(User User, string Token)> Authenticate(string login, string passwordText);

    public Maybe<User> GetCurrentUser();
}
