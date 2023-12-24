using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Application.Services.Diagrams;
using DiagramEditor.Domain.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Create;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class CreateDiagramUseCase(
    IDiagramRepository diagrams,
    IAuthenticator auth,
    IDiagramNameValidator nameValidator
) : ICreateDiagramUseCase
{
    public Task<Result<CreateDiagramResponse, EnumError<CreateDiagramError>>> Execute(
        CreateDiagramRequest request
    )
    {
        return nameValidator
            .Validate(request.Name)
            .ToFailure()
            .MapError(
                validationError =>
                    EnumError.From(CreateDiagramError.ValidationError, validationError.Messages)
            )
            .Bind(
                _ =>
                    auth.GetAuthenticatedUser()
                        .ToResult(CreateDiagramError.Unauthorized)
                        .MapError(EnumError.From)
                        .Map(
                            user =>
                                new Diagram
                                {
                                    User = user,
                                    Name = request.Name,
                                    Description = request.Description.GetValueOrDefault(""),
                                }
                        )
                        .Tap(diagrams.Add)
                        .Map(DiagramDTO.FromDiagram)
                        .Map(diagramDto => new CreateDiagramResponse(diagramDto))
            )
            .ToCompletedTask();
    }
}
