using CSharpFunctionalExtensions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.DTOs;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.UseCases.Diagrams.FindPublic;

[Injectable(ServiceLifetime.Singleton)]
internal sealed record FindPublicDiagramsUseCase(IDiagramEnvironmentRepository environments)
    : IFindPublicDiagramsUseCase
{
    public Task<Result<FindPublicDiagramsResponse, EnumError<FindPublicDiagramsError>>> Execute(
        FindPublicDiagramsRequest request
    )
    {
        return request
            .Query.AsMaybe()
            .Where(query => query.Length > 0)
            .ToResult(FindPublicDiagramsError.EmptyQuery)
            .Map(query => query.ToLower())
            .Map(
                query =>
                    environments
                        .GetPublicAndActive()
                        .Select(env => env.Diagram)
                        .Where(
                            diagram =>
                                diagram.Name.ToLower().Contains(query)
                                || diagram.Description.ToLower().Contains(query)
                        )
                        .Select(DiagramDTO.FromDiagram)
                        .ToArray()
            )
            .Map(diagramDtos => new FindPublicDiagramsResponse(diagramDtos))
            .MapError(EnumError.From)
            .ToCompletedTask();
    }
}
