using CSharpFunctionalExtensions;
using DiagramEditor.Application;
using DiagramEditor.Application.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal class RefreshTokenRepository(IDistributedCache cache) : IRefreshTokenRepository
{
    public Maybe<string> GetToken(Guid userId)
    {
        return cache.GetString(GuidToKey(userId)).AsMaybe();
    }

    public void SetToken(Guid userId, string token, DateTime expirationDate)
    {
        cache.SetString(GuidToKey(userId), token, new() { AbsoluteExpiration = expirationDate, });
    }

    public void DeleteToken(Guid userId)
    {
        cache.Remove(GuidToKey(userId));
    }

    private static string GuidToKey(Guid userId)
    {
        return $"refresh.{userId}";
    }
}
