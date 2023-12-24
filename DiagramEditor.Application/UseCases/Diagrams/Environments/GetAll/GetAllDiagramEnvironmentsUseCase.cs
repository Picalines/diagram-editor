using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.GetAll;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class GetAllDiagramEnvironmentsUseCase(
    IDiagramRepository diagrams,
    IDiagramEnvironmentRepository environments,
    IAuthenticator auth
) : IGetAllDiagramEnvironmentsUseCase
{
    public Task<
        Result<GetAllDiagramEnvironmentsResponse, EnumError<GetAllDiagramEnvironmentsError>>
    > Execute(GetAllDiagramEnvironmentsRequest request)
    {
        return auth.GetAuthenticatedUser()
            .ToResult(GetAllDiagramEnvironmentsError.DiagramNotFound)
            .Bind(
                user =>
                    diagrams
                        .GetById(request.DiagramId)
                        .Where(diagram => diagram.User.Id == user.Id)
                        .ToResult(GetAllDiagramEnvironmentsError.DiagramNotFound)
            )
            .Map(
                diagram =>
                    environments
                        .GetAllByDiagram(diagram)
                        .Select(DiagramEnvironmentDTO.FromEnvironment)
                        .ToArray()
            )
            .Map(envDtos => new GetAllDiagramEnvironmentsResponse(envDtos))
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
