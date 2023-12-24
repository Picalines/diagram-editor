using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;
using DiagramEditor.Application.UseCases.Diagrams.FindPublic;
using DiagramEditor.Application.UseCases.Diagrams.GetInfo;
using DiagramEditor.Domain.Diagrams;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("diagrams")]
public sealed class PublicDiagramsController(
    IGetDiagramInfoUseCase getUseCase,
    IFindPublicDiagramsUseCase findPublicUseCase,
    IGetAllDiagramElementsUseCase getElementsUseCase
) : ControllerBase
{
    [HttpGet("{environmentId}")]
    public async Task<
        Results<Ok<GetDiagramInfoResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetDiagramById([FromRoute] Guid environmentId) =>
        await getUseCase.Execute(
            new GetDiagramInfoRequest
            {
                Id = environmentId,
                ViewMode = DiagramViewMode.FromEnvironment,
            }
        ) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    GetDiagramInfoError.Unauthorized => TypedResults.Unauthorized(),
                    GetDiagramInfoError.NotFound => TypedResults.NotFound(),
                    _ => throw new NotImplementedException(),
                },
        };

    [HttpGet]
    public async Task<Results<Ok<FindPublicDiagramsResponse>, BadRequest>> FindPublicDiagrams(
        [FromQuery] string query
    ) =>
        await findPublicUseCase.Execute(new FindPublicDiagramsRequest { Query = query }) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    FindPublicDiagramsError.EmptyQuery => TypedResults.BadRequest(),
                    _ => throw new NotImplementedException(),
                },
        };

    [HttpGet("{environmentId}/elements")]
    public async Task<
        Results<Ok<GetAllDiagramElementsResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetAllPublicDiagramElements([FromRoute, Required] Guid environmentId) =>
        await getElementsUseCase.Execute(
            new GetAllDiagramElementsRequest
            {
                Id = environmentId,
                ViewMode = DiagramViewMode.FromEnvironment,
            }
        ) switch
        {
            { IsSuccess: true, Value: var value } => TypedResults.Ok(value),
            { Error.Error: var error }
                => error switch
                {
                    GetAllDiagramElementsError.DiagramNotFound => TypedResults.NotFound(),
                    GetAllDiagramElementsError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };
}
