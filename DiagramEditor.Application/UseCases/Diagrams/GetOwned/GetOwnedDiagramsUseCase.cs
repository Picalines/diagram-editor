using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.GetOwned;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class GetOwnedDiagramUseCase(IDiagramRepository diagrams, IAuthenticator auth)
    : IGetOwnedDiagramsUseCase
{
    public Task<Result<GetOwnedDiagramsResponse, EnumError<GetOwnedDiagramsError>>> Execute(
        Unit request
    )
    {
        return auth.GetAuthenticatedUser()
            .ToResult(GetOwnedDiagramsError.Unauthorized)
            .Map(user => diagrams.GetCreatedByUser(user))
            .Map(diagrams => diagrams.Select(DiagramDTO.FromDiagram))
            .Map(diagramDtos => new GetOwnedDiagramsResponse(diagramDtos.ToArray()))
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
