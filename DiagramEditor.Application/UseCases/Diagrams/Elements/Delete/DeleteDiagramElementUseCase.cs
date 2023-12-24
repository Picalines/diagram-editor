using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Elements.Delete;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DeleteDiagramElementUseCase(
    IDiagramElementRepository elements,
    IAuthenticator auth
) : IDeleteDiagramElementUseCase
{
    public Task<Result<Unit, EnumError<DeleteDiagramElementError>>> Execute(
        DeleteDiagramElementRequest request
    )
    {
        return auth.GetAuthenticatedUser()
            .ToResult(DeleteDiagramElementError.Unauthorized)
            .Bind(
                user =>
                    elements
                        .GetById(request.Id)
                        .Where(d => d.Diagram.User.Id == user.Id)
                        .ToResult(DeleteDiagramElementError.NotFound)
            )
            .Tap(elements.Remove)
            .Map(_ => Unit.Instance)
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
