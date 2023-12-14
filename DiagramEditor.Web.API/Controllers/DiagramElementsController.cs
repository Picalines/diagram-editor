using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application.UseCases.Diagrams.Elements.Create;
using DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams/{diagramId}/elements")]
public sealed class DiagramElementsController(
    IGetAllDiagramElementsUseCase getAllUseCase,
    ICreateDiagramElementUseCase createUseCase
) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<
        Results<Ok<GetAllDiagramElementsResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetAllUserDiagramElements([FromQuery, Required] Guid diagramId) =>
        await getAllUseCase.Execute(
            new GetAllDiagramElementsRequest { Id = diagramId, ViewMode = DiagramViewMode.InEditor }
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

    [Authorize]
    [HttpPost]
    public async Task<
        Results<Ok<CreateDiagramElementResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > CreateDiagramElement(
        [FromBody, Required] CreateDiagramElementRequest request,
        [FromQuery, Required] Guid diagramId
    ) =>
        await createUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var value } => TypedResults.Ok(value),
            { Error.Error: var error }
                => error switch
                {
                    CreateDiagramElementError.DiagramNotFound => TypedResults.NotFound(),
                    CreateDiagramElementError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                }
        };
}
