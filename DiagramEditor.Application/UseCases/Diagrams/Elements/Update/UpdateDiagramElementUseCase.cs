using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.UseCase;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UpdateDiagramElementUseCase(
    IDiagramElementRepository elements,
    IDiagramElementPropertyRepository properties,
    IAuthenticator auth
) : IUpdateDiagramElementUseCase
{
    public Task<Result<UpdateDiagramElementResponse, EnumError<UpdateDiagramElementError>>> Execute(
        UpdateDiagramElementRequest request
    )
    {
        return auth.GetAuthenticatedUser()
            .ToResult(UpdateDiagramElementError.Unauthorized)
            .Bind(
                user =>
                    elements
                        .GetById(request.Id)
                        .Where(element => element.Diagram.User.Id == user.Id)
                        .ToResult(UpdateDiagramElementError.NotFound)
            )
            .Tap(element =>
            {
                request.OriginX.Execute(x => element.OriginX = x);
                request.OriginY.Execute(y => element.OriginY = y);
            })
            .Tap(elements.Update)
            .Tap(
                element =>
                    request
                        .Properties.Tap(_ => properties.RemoveAllByElement(element))
                        .Map(
                            newProperties =>
                                newProperties.Select(
                                    property =>
                                        new DiagramElementProperty
                                        {
                                            Element = element,
                                            Key = property.Key,
                                            Value = property.Value
                                        }
                                )
                        )
                        .Tap(properties.AddRange)
            )
            .Map(element => DiagramElementDTO.FromElement(element, properties))
            .Map(elementDto => new UpdateDiagramElementResponse(elementDto))
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
