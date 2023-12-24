using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.Create;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class CreateDiagramElementUseCase(
    IDiagramRepository diagrams,
    IDiagramElementRepository elements,
    IDiagramElementPropertyRepository properties,
    IAuthenticator auth
) : ICreateDiagramElementUseCase
{
    public Task<Result<CreateDiagramElementResponse, EnumError<CreateDiagramElementError>>> Execute(
        CreateDiagramElementRequest request
    )
    {
        return auth.GetAuthenticatedUser()
            .ToResult(CreateDiagramElementError.Unauthorized)
            .Bind(
                user =>
                    diagrams
                        .GetById(request.DiagramId)
                        .Where(diagram => diagram.User.Id == user.Id)
                        .ToResult(CreateDiagramElementError.DiagramNotFound)
                        .Map(
                            diagram =>
                                new DiagramElement
                                {
                                    Diagram = diagram,
                                    Type = request.Type,
                                    OriginX = request.OriginX,
                                    OriginY = request.OriginY,
                                }
                        )
                        .Tap(elements.Add)
                        .Tap(
                            element =>
                                properties.AddRange(
                                    request.Properties.Select(
                                        property =>
                                            new DiagramElementProperty
                                            {
                                                Element = element,
                                                Key = property.Key,
                                                Value = property.Value
                                            }
                                    )
                                )
                        )
                        .Map(element => DiagramElementDTO.Create(element, properties))
                        .Map(elementDto => new CreateDiagramElementResponse(elementDto))
            )
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
