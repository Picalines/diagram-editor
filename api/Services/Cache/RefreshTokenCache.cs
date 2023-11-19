using DiagramEditor.Attributes;
using Microsoft.Extensions.Caching.Distributed;

namespace DiagramEditor.Services.Cache;

[Injectable(ServiceLifetime.Singleton)]
public sealed class RefreshTokenCache(IDistributedCache cache)
    : TokenCache(cache, "refresh"),
        IRefreshTokenCache { }
