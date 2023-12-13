using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application.UseCases.Diagrams.Create;
using DiagramEditor.Application.UseCases.Diagrams.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams")]
public sealed class UserDiagramsController(
    ICreateDiagramUseCase createUseCase,
    IDeleteDiagramUseCase deleteUseCase
) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<
        Results<Ok<CreateDiagramResponse>, BadRequest, UnauthorizedHttpResult>
    > CreateDiagram([FromBody, Required] CreateDiagramRequest request)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        return await createUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    CreateDiagramError.ValidationError => TypedResults.BadRequest(),
                    CreateDiagramError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                }
        };
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<Results<Ok, BadRequest, NotFound, UnauthorizedHttpResult>> DeleteDiagram(
        [FromQuery, Required] Guid diagramId
    )
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        return await deleteUseCase.Execute(new DeleteDiagramRequest { Id = diagramId }) switch
        {
            { IsSuccess: true } => TypedResults.Ok(),
            { Error.Error: var error }
                => error switch
                {
                    DeleteDiagramError.Unauthorized => TypedResults.Unauthorized(),
                    DeleteDiagramError.NotFound => TypedResults.NotFound(),
                    _ => throw new NotImplementedException(),
                }
        };
    }
}
