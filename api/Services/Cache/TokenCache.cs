using CSharpFunctionalExtensions;
using DiagramEditor.Database.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace DiagramEditor.Services.Cache;

public class TokenCache(IDistributedCache cache, string tokenPrefix) : ITokenCache
{
    public Maybe<string> GetToken(User user)
    {
        return cache.GetString(GetCacheKey(user)).AsMaybe();
    }

    public void SetToken(User user, string refreshToken, DateTime expirationDate)
    {
        cache.SetString(
            GetCacheKey(user),
            refreshToken,
            new() { AbsoluteExpiration = expirationDate }
        );
    }

    public void DeleteToken(User user)
    {
        cache.Remove(GetCacheKey(user));
    }

    private string GetCacheKey(User user)
    {
        return $"{tokenPrefix}.{user.Id}";
    }
}
