using CSharpFunctionalExtensions;
using DiagramEditor.Application;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Domain.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Repositories;

[Injectable(ServiceLifetime.Singleton)]
internal class RefreshTokenRepository(IDistributedCache cache) : IRefreshTokenRepository
{
    public Maybe<string> GetToken(UserId userId)
    {
        return cache.GetString(UserIdToKey(userId)).AsMaybe();
    }

    public void SetToken(UserId userId, string token, DateTime expirationDate)
    {
        cache.SetString(UserIdToKey(userId), token, new()
        {
            AbsoluteExpiration = expirationDate,
        });
    }

    public void DeleteToken(UserId userId)
    {
        cache.Remove(UserIdToKey(userId));
    }

    private static string UserIdToKey(UserId userId)
    {
        return $"refresh.{userId}";
    }
}
