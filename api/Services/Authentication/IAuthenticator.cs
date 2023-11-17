using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Services.Authentication;

using AuthTokens = (string AccessToken, string RefreshToken);

public interface IAuthenticator
{
    public Maybe<User> IdentifyUser(string login, string passwordText);

    public AuthTokens Authenticate(User user);

    public bool ValidateRefresh(User user, string refreshToken);

    public void Deauthenticate(User user);

    public Maybe<User> GetCurrentUser();

    public Maybe<AuthTokens> Reauthenticate(User user, string refreshToken)
    {
        return ValidateRefresh(user, refreshToken) ? Authenticate(user) : Maybe.None;
    }
}
