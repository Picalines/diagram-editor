using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Services.Authentication;

public interface IAuthenticator
{
    public Maybe<User> Identify(string login, string passwordText);

    public Maybe<(string AccessToken, string RefreshToken)> Authenticate(User user);

    public Maybe<User> GetCurrentUser();
}
