using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class GetAllDiagramElementsUseCase(
    IDiagramRepository diagrams,
    IDiagramEnvironmentRepository environments,
    IDiagramElementRepository elements,
    IDiagramElementPropertyRepository properties,
    IAuthenticator auth
) : IGetAllDiagramElementsUseCase
{
    public Task<
        Result<GetAllDiagramElementsResponse, EnumError<GetAllDiagramElementsError>>
    > Execute(GetAllDiagramElementsRequest request)
    {
        return GetDiagram(request)
            .Map(elements.GetAllByDiagram)
            .Map(
                elements =>
                    elements.Select(element => DiagramElementDTO.Create(element, properties))
            )
            .Map(elementDtos => new GetAllDiagramElementsResponse(elementDtos.ToArray()))
            .MapError(EnumError.From)
            .ToCompletedTask();
    }

    private Result<Diagram, GetAllDiagramElementsError> GetDiagram(
        GetAllDiagramElementsRequest request
    )
    {
        return request.ViewMode switch
        {
            DiagramViewMode.InEditor
                => auth.GetAuthenticatedUser()
                    .ToResult(GetAllDiagramElementsError.Unauthorized)
                    .Bind(
                        user =>
                            diagrams
                                .GetById(request.Id)
                                .Where(diagram => diagram.Creator.Id == user.Id)
                                .ToResult(GetAllDiagramElementsError.DiagramNotFound)
                    ),

            DiagramViewMode.FromEnvironment
                => environments
                    .GetById(request.Id)
                    .Where(env => env.IsActive)
                    .Map(env => env.Diagram)
                    .ToResult(GetAllDiagramElementsError.DiagramNotFound),

            _ => throw new NotImplementedException(),
        };
    }
}
