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

namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UpdateCurrentUserUseCase(
    IAuthenticator auth,
    IUserRepository users,
    ILoginValidator loginValidator,
    IPasswordValidator passwordValidator,
    IPasswordHasher passwordHasher
) : IUpdateCurrentUserUseCase
{
    public Task<Result<UpdateCurrentUserResponse, EnumError<UpdateCurrentUserError>>> Execute(
        UpdateCurrentUserRequest request
    )
    {
        return request
            .Login
            .Bind(loginValidator.Validate)
            .Bind(_ => request.Password.Bind(passwordValidator.Validate))
            .ToFailure()
            .MapError(
                validationError =>
                    EnumError.From(UpdateCurrentUserError.ValidationError, validationError.Messages)
            )
            .Check(
                _ =>
                    request
                        .Login
                        .Bind(users.GetByLogin)
                        .ToFailure()
                        .MapError(_ => UpdateCurrentUserError.LoginTaken)
                        .MapError(EnumError.From)
            )
            .Bind(
                _ =>
                    auth.GetAuthenticatedUser()
                        .ToResult(UpdateCurrentUserError.Unauthorized)
                        .Tap(user => AssignUserFields(user, request))
                        .Tap(users.Update)
                        .Map(UserDTO.FromUser)
                        .Map(userDto => new UpdateCurrentUserResponse(userDto))
                        .MapError(EnumError.From)
            )
            .ToCompletedTask();
    }

    private void AssignUserFields(User user, UpdateCurrentUserRequest request)
    {
        request.Login.Execute(login => user.Login = login);

        request.Password.Execute(password => user.PasswordHash = passwordHasher.Hash(password));

        request.DisplayName.Execute(displayName => user.DisplayName = displayName);

        request
            .AvatarUrl
            .Execute(avatarUrl => user.AvatarUrl = avatarUrl.Length > 0 ? avatarUrl : null);
    }
}
