using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Application.Services.Diagrams;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.UpdateInfo;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class UpdateDiagramInfoUseCase(
    IDiagramRepository diagrams,
    IAuthenticator auth,
    IDiagramNameValidator nameValidator
) : IUpdateDiagramInfoUseCase
{
    public Task<Result<UpdateDiagramInfoResponse, EnumError<UpdateDiagramInfoError>>> Execute(
        UpdateDiagramInfoRequest request
    )
    {
        return request
            .Name
            .Bind(nameValidator.Validate)
            .ToFailure()
            .MapError(
                validationError =>
                    EnumError.From(UpdateDiagramInfoError.ValidationError, validationError.Messages)
            )
            .Bind(
                _ =>
                    auth.GetAuthenticatedUser()
                        .ToResult(UpdateDiagramInfoError.Unauthorized)
                        .Bind(
                            user =>
                                diagrams
                                    .GetById(request.Id)
                                    .Where(diagram => diagram.User.Id == user.Id)
                                    .ToResult(UpdateDiagramInfoError.NotFound)
                        )
                        .Tap(diagram =>
                        {
                            request.Name.Execute(name => diagram.Name = name);
                            request.Description.Execute(desc => diagram.Description = desc);
                            diagram.UpdatedDate = DateTime.UtcNow;
                        })
                        .Tap(diagrams.Update)
                        .Map(DiagramDTO.FromDiagram)
                        .Map(diagramDto => new UpdateDiagramInfoResponse(diagramDto))
                        .MapError(EnumError.From)
            )
            .ToCompletedTask();
    }
}
