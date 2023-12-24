using DiagramEditor.Application.UseCases.Diagrams.Elements.Create;
using DiagramEditor.Application.UseCases.Diagrams.Elements.Delete;
using DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Web.API.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams/{diagramId}/elements")]
public sealed class DiagramElementsController(
    IGetAllDiagramElementsUseCase getAllUseCase,
    ICreateDiagramElementUseCase createUseCase,
    IDeleteDiagramElementUseCase deleteUseCase
) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<
        Results<Ok<GetAllDiagramElementsResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetAllUserDiagramElements([FromRoute] Guid diagramId) =>
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
        [FromRoute] Guid diagramId,
        [FromBody] CreateDiagramElementRequestDTO request
    ) =>
        await createUseCase.Execute(
            new CreateDiagramElementRequest
            {
                DiagramId = diagramId,
                Type = request.Type,
                OriginX = request.OriginX,
                OriginY = request.OriginY,
                Properties = request.Properties
            }
        ) switch
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

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<
        Results<Ok, BadRequest, NotFound, UnauthorizedHttpResult>
    > DeleteDiagramElement([FromRoute] Guid diagramId, [FromRoute] Guid id) =>
        await deleteUseCase.Execute(new DeleteDiagramElementRequest { Id = id }) switch
        {
            { IsSuccess: true } => TypedResults.Ok(),
            { Error.Error: var error }
                => error switch
                {
                    DeleteDiagramElementError.NotFound => TypedResults.NotFound(),
                    DeleteDiagramElementError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                }
        };
}
