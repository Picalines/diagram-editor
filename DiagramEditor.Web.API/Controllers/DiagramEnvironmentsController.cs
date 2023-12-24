using DiagramEditor.Application.UseCases.Diagrams.Environments.Create;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams/{diagramId}/envrionments")]
public sealed class DiagramEnvironmentsController(ICreateDiagramEnvironmentUseCase createUseCase)
    : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<
        Results<Ok<CreateDiagramEnvironmentResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetAllUserDiagramElements([FromHybrid] CreateDiagramEnvironmentRequest request) =>
        await createUseCase.Execute(request) switch
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