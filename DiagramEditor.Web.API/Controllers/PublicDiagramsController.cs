using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application.UseCases.Diagrams.Elements.GetAll;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("diagrams/{environmentId}")]
public sealed class PublicDiagramsController(IGetAllDiagramElementsUseCase getElementsUseCase)
    : ControllerBase
{
    [HttpGet("elements")]
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
