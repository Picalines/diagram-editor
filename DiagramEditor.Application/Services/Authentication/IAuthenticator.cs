using CSharpFunctionalExtensions;
using DiagramEditor.Domain;

namespace DiagramEditor.Application.Services.Authentication;

using DiagramEditor.Domain.Users;

public interface IAuthenticator
{
    public Maybe<User> IdentifyUser(string login, string password);

    public Maybe<User> IdentifyUser(AuthTokens authTokens);

    public AuthTokens Authenticate(User user);

    public bool ValidateRefresh(User user, string refreshToken);

    public void Deauthenticate(User user);

    public Maybe<User> GetAuthenticatedUser();

    public Maybe<AuthTokens> Reauthenticate(User user, string refreshToken)
    {
        return ValidateRefresh(user, refreshToken) ? Authenticate(user) : Maybe.None;
    }
}
