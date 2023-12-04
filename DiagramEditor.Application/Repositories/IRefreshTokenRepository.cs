using CSharpFunctionalExtensions;
using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application;

public interface IRefreshTokenRepository
{
    public Maybe<string> GetToken(UserId userId);

    public void SetToken(UserId userId, string token, DateTime expirationDate);

    public void DeleteToken(UserId userId);
}
