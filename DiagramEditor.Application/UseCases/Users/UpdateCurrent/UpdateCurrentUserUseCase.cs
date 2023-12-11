using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Application.Services.Passwords;
using DiagramEditor.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UpdateCurrentUserUseCase(
    IAuthenticator auth,
    IUserRepository users,
    ILoginValidator loginValidator,
    IPasswordValidator passwordValidator
) : IUpdateCurrentUserUseCase
{
    public Task<Result<UpdateCurrentUserResponse, EnumError<UpdateCurrentUserError>>> Execute(
        UpdateCurrentUserRequest request
    )
    {
        if (
            request.Login.TryGetValue(out var login)
            && loginValidator.Validate(login, out var loginErrors) is false
        )
        {
            return Result
                .Failure<UpdateCurrentUserResponse, UpdateCurrentUserError>(
                    UpdateCurrentUserError.ValidationError
                )
                .MapError(error => EnumError.From(error, loginErrors))
                .ToCompletedTask();
        }

        if (
            request.Password.TryGetValue(out var password)
            && passwordValidator.Validate(password, out var passwordErrors) is false
        )
        {
            return Result
                .Failure<UpdateCurrentUserResponse, UpdateCurrentUserError>(
                    UpdateCurrentUserError.ValidationError
                )
                .MapError(error => EnumError.From(error, passwordErrors))
                .ToCompletedTask();
        }

        return auth.GetAuthenticatedUser()
            .ToResult(UpdateCurrentUserError.Unauthorized)
            .Bind(user =>
            {
                var updatedUser = new UpdateUserDto()
                {
                    Login = request.Login,
                    Password = request.Password,
                    DisplayName = request.DisplayName,
                    AvatarUrl = request.AvatarUrl,
                };

                return users
                    .Update(user, updatedUser)
                    .Map(user => new UpdateCurrentUserResponse(user))
                    .MapError(error => error.CastTo<UpdateCurrentUserError>());
            })
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
