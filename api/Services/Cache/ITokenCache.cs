
using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;

namespace DiagramEditor.Services.Cache;

public interface ITokenCache
{
    public Maybe<string> GetToken(User user);

    public void SetToken(User user, string token, DateTime expirationDate);

    public void DeleteToken(User user);
}

