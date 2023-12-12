using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Application.Services.Passwords;
using DiagramEditor.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Users.Register;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class RegisterUseCase(
    IAuthenticator auth,
    IUserRepository users,
    ILoginValidator loginValidator,
    IPasswordValidator passwordValidator
) : IRegisterUseCase
{
    public Task<Result<RegisterResponse, EnumError<RegisterError>>> Execute(RegisterRequest request)
    {
        return loginValidator
            .Validate(request.Login)
            .Bind(_ => passwordValidator.Validate(request.Password))
            .ToFailure()
            .MapError(
                validationError =>
                    EnumError.From(RegisterError.ValidationError, validationError.Messages)
            )
            .Bind(
                _ =>
                    users
                        .Register(request.Login, request.Password)
                        .MapError(error => error.CastTo<RegisterError>())
                        .MapError(EnumError.From)
                        .Map(auth.Authenticate)
                        .Map(
                            tokens =>
                                new RegisterResponse
                                {
                                    AccessToken = tokens.AccessToken,
                                    RefreshToken = tokens.RefreshToken
                                }
                        )
            )
            .ToCompletedTask();
    }
}
