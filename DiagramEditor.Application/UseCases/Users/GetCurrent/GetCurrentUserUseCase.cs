using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Users.GetCurrent;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class GetCurrentUserUseCase(IAuthenticator auth) : IGetCurrentUserUseCase
{
    public Task<Result<CurrentUserResponse, EnumError<GetCurrentUserError>>> Execute(Unit request)
    {
        return auth.GetAuthenticatedUser()
            .ToResult(GetCurrentUserError.Unauthorized)
            .MapError(EnumError.From)
            .Map(UserDTO.FromUser)
            .Map(userDto => new CurrentUserResponse(userDto))
            .ToCompletedTask();
    }
}
