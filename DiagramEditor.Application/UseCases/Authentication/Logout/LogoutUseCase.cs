using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Authentication.Logout;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class LogoutUseCase(IAuthenticator auth) : ILogoutUseCase
{
    public Task<Result<Unit, EnumError<LogoutError>>> Execute(Unit request)
    {
        return auth.GetAuthenticatedUser()
            .ToResult(LogoutError.NotAuthenticated)
            .MapError(EnumError.From)
            .Map(user =>
            {
                auth.Deauthenticate(user);
                return Unit.Instance;
            })
            .ToCompletedTask();
    }
}
