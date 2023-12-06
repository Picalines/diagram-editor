using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Authentication.Refresh;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class RefreshUseCase(IAuthenticator auth) : IRefreshUseCase
{
    public Task<Result<RefreshResponse, EnumError<RefreshError>>> Execute(RefreshRequest request)
    {
        return auth
            .IdentifyUser(new AuthTokens(request.AccessToken, request.RefreshToken))
            .ToResult(RefreshError.InvalidCredentials)
            .MapError(EnumError.From)
            .Bind(user =>
                auth.Reauthenticate(user, request.RefreshToken)
                    .ToResult(RefreshError.RefreshExpired)
                    .MapError(EnumError.From))
            .Map(tokens => new RefreshResponse(tokens.AccessToken, tokens.RefreshToken))
            .ToCompletedTask();
    }
}
