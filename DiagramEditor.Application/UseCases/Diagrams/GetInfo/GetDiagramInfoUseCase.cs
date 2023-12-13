using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.GetInfo;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class GetDiagramInfoUseCase(IDiagramRepository diagrams, IAuthenticator auth)
    : IGetDiagramInfoUseCase
{
    public Task<Result<GetDiagramInfoResponse, EnumError<GetDiagramInfoError>>> Execute(
        GetDiagramInfoRequest request
    )
    {
        return auth.GetAuthenticatedUser()
            .ToResult(GetDiagramInfoError.Unauthorized)
            .Bind(
                user =>
                    diagrams
                        .GetById(request.Id)
                        .Where(diagram => diagram.Creator.Id == request.Id)
                        .ToResult(GetDiagramInfoError.NotFound)
                        .Map(diagram => new GetDiagramInfoResponse(diagram))
            )
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
