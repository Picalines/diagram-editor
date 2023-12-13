using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
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
                                    .Where(diagram => diagram.Creator.Id == user.Id)
                                    .ToResult(UpdateDiagramInfoError.NotFound)
                        )
                        .Map(
                            diagram =>
                                diagrams.Update(
                                    diagram,
                                    new DiagramUpdateDto
                                    {
                                        Name = request.Name,
                                        Description = request.Description
                                    }
                                )
                        )
                        .Map(diagram => new UpdateDiagramInfoResponse(diagram))
                        .MapError(EnumError.From)
            )
            .ToCompletedTask();
    }
}
