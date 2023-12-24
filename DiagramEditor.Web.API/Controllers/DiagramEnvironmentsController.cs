using DiagramEditor.Application.UseCases.Diagrams.Environments.Create;
using DiagramEditor.Web.API.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams/{diagramId}/environments")]
public sealed class DiagramEnvironmentsController(ICreateDiagramEnvironmentUseCase createUseCase)
    : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<
        Results<Ok<CreateDiagramEnvironmentResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetAllUserDiagramElements(
        [FromRoute] Guid diagramId,
        [FromBody] CreateDiagramEnvironmentRequestDTO request
    ) =>
        await createUseCase.Execute(
            new CreateDiagramEnvironmentRequest
            {
                DiagramId = diagramId,
                IsPublic = request.IsPublic
            }
        ) switch
        {
            { IsSuccess: true, Value: var value } => TypedResults.Ok(value),
            { Error.Error: var error }
                => error switch
                {
                    CreateDiagramEnvironmentError.DiagramNotFound => TypedResults.NotFound(),
                    CreateDiagramEnvironmentError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };
}
