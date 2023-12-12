using CSharpFunctionalExtensions;

namespace DiagramEditor.Application;

public interface IRefreshTokenRepository
{
    public Maybe<string> GetToken(Guid userId);

    public void SetToken(Guid userId, string token, DateTime expirationDate);

    public void DeleteToken(Guid userId);
}
