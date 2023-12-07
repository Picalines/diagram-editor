using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Authentication.Login;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class LoginUseCase(IAuthenticator auth) : ILoginUseCase
{
    public Task<Result<LoginResponse, EnumError<LoginError>>> Execute(LoginRequest request)
    {
        return auth.IdentifyUser(request.Login, request.Password)
            .Map(auth.Authenticate)
            .ToResult(LoginError.InvalidCredentials)
            .MapError(EnumError.From)
            .Map(tokens => new LoginResponse(tokens.AccessToken, tokens.RefreshToken))
            .ToCompletedTask();
    }
}
