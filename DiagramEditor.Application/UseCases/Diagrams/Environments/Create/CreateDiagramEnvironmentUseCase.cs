using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Environments.Create;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class CreateDiagramEnvironmentUseCase(
    IDiagramRepository diagrams,
    IDiagramEnvironmentRepository environments,
    IAuthenticator auth
) : ICreateDiagramEnvironmentUseCase
{
    public Task<
        Result<CreateDiagramEnvironmentResponse, EnumError<CreateDiagramEnvironmentError>>
    > Execute(CreateDiagramEnvironmentRequest request)
    {
        return auth.GetAuthenticatedUser()
            .ToResult(CreateDiagramEnvironmentError.Unauthorized)
            .Bind(
                user =>
                    diagrams
                        .GetById(request.DiagramId)
                        .ToResult(CreateDiagramEnvironmentError.DiagramNotFound)
                        .Map(
                            diagram =>
                                new DiagramEnvironment
                                {
                                    Diagram = diagram,
                                    IsActive = false,
                                    ViewsCount = 0,
                                }
                        )
                        .Tap(environments.Add)
                        .Map(DiagramEnvironmentDTO.FromEnvironment)
                        .Map(envDto => new CreateDiagramEnvironmentResponse(envDto))
            )
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
