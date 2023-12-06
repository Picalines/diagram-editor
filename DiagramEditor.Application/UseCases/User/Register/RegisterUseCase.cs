using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.User.Register;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class RegisterUseCase(IAuthenticator auth, IUserRepository users) : IRegisterUseCase
{
    public Task<Result<RegisterResponse, EnumError<RegisterError>>> Execute(RegisterRequest request)
    {
        return users
            .Register(request.Login, request.Password)
            .MapError(error => EnumError.From((RegisterError)error))
            .Map(auth.Authenticate)
            .Map(tokens => new RegisterResponse { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken })
            .ToCompletedTask();
    }
}
