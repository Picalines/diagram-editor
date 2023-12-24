using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.Update;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UpdateDiagramEnvironmentUseCase(
    IDiagramEnvironmentRepository environments,
    IAuthenticator auth
) : IUpdateDiagramEnvironmentUseCase
{
    public Task<
        Result<UpdateDiagramEnvironmentResponse, EnumError<UpdateDiagramEnvironmentError>>
    > Execute(UpdateDiagramEnvironmentRequest request)
    {
        return auth.GetAuthenticatedUser()
            .ToResult(UpdateDiagramEnvironmentError.Unauthorized)
            .Bind(
                user =>
                    environments
                        .GetById(request.Id)
                        .Where(environment => environment.Diagram.User.Id == user.Id)
                        .ToResult(UpdateDiagramEnvironmentError.NotFound)
            )
            .Tap(environment =>
            {
                request.IsActive.Execute(isActive => environment.IsActive = isActive);
                request.ViewsCount.Execute(count => environment.ViewsCount = count);
            })
            .Tap(environments.Update)
            .Map(DiagramEnvironmentDTO.FromEnvironment)
            .Map(envDto => new UpdateDiagramEnvironmentResponse(envDto))
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
