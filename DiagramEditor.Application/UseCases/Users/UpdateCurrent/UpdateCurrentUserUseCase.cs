namespace DiagramEditor.Application.UseCases.Users.UpdateCurrent;

using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UpdateCurrentUserUseCase(IAuthenticator auth, IUserRepository users)
    : IUpdateCurrentUserUseCase
{
    public Task<Result<UpdateCurrentUserResponse, EnumError<UpdateCurrentUserError>>> Execute(
        UpdateCurrentUserRequest request
    )
    {
        if (auth.GetAuthenticatedUser().TryGetValue(out var user) is false)
        {
            return Task.FromResult(
                (Result<UpdateCurrentUserResponse, EnumError<UpdateCurrentUserError>>)
                    EnumError.From(UpdateCurrentUserError.Unauthorized)
            );
        }

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
            .MapError(error => (UpdateCurrentUserError)error)
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
