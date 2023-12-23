using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Application.Services.Passwords;
using DiagramEditor.Application.Services.Users;
using DiagramEditor.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Users.Register;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class RegisterUseCase(
    IAuthenticator auth,
    IUserRepository users,
    ILoginValidator loginValidator,
    IPasswordValidator passwordValidator,
    IPasswordHasher passwordHasher
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
            .Check(
                _ =>
                    users
                        .GetByLogin(request.Login)
                        .ToFailure()
                        .MapError(_ => RegisterError.LoginTaken)
                        .MapError(EnumError.From)
            )
            .Map(
                _ =>
                    new User
                    {
                        Login = request.Login,
                        PasswordHash = passwordHasher.Hash(request.Password),
                        DisplayName = request.Login,
                    }
            )
            .Tap(users.Add)
            .Map(user => new { User = UserDTO.FromUser(user), Tokens = auth.Authenticate(user) })
            .Map(
                pair =>
                    new RegisterResponse
                    {
                        AccessToken = pair.Tokens.AccessToken,
                        RefreshToken = pair.Tokens.RefreshToken,
                        User = pair.User,
                    }
            )
            .ToCompletedTask();
    }
}
