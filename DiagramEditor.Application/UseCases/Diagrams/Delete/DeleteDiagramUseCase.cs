using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.Delete;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DeleteDiagramUseCase(IDiagramRepository diagrams, IAuthenticator auth)
    : IDeleteDiagramUseCase
{
    public Task<Result<Unit, EnumError<DeleteDiagramError>>> Execute(DeleteDiagramRequest request)
    {
        return auth.GetAuthenticatedUser()
            .ToResult(DeleteDiagramError.Unauthorized)
            .Bind(
                user =>
                    diagrams
                        .GetById(request.Id)
                        .Where(diagram => diagram.Creator.Id == user.Id)
                        .ToResult(DeleteDiagramError.NotFound)
                        .Tap(diagrams.Remove)
                        .Map(_ => Unit.Instance)
            )
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
