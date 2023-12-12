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
        return request
            .Login
            .Bind(loginValidator.Validate)
            .Bind(_ => request.Password.Bind(passwordValidator.Validate))
            .ToFailure()
            .MapError(
                validationError =>
                    EnumError.From(UpdateCurrentUserError.ValidationError, validationError.Messages)
            )
            .Bind(
                _ =>
                    auth.GetAuthenticatedUser()
                        .ToResult(UpdateCurrentUserError.Unauthorized)
                        .Bind(user =>
                        {
                            var updatedUser = new UpdateUserDto()
                            {
                                Login = request.Login,
                                Password = request.Password,
                                DisplayName = request.DisplayName,
                                AvatarUrl = request
                                    .AvatarUrl
                                    .Map(
                                        avatarUrl =>
                                            Maybe.From(avatarUrl).Where(_ => avatarUrl is not "")
                                    ),
                            };

                            return users
                                .Update(user, updatedUser)
                                .Map(user => new UpdateCurrentUserResponse(user))
                                .MapError(error => error.CastTo<UpdateCurrentUserError>());
                        })
                        .MapError(EnumError.From)
            )
            .ToCompletedTask();
    }
}
