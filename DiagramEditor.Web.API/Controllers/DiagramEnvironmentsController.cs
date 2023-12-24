using DiagramEditor.Application.UseCases.Diagrams.Environments.Create;
using DiagramEditor.Application.UseCases.Diagrams.Environments.GetAll;
using DiagramEditor.Application.UseCases.Diagrams.Environments.Update;
using DiagramEditor.Web.API.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams/{diagramId}/environments")]
public sealed class DiagramEnvironmentsController(
    IGetAllDiagramEnvironmentsUseCase getAllUseCase,
    ICreateDiagramEnvironmentUseCase createUseCase,
    IUpdateDiagramEnvironmentUseCase updateUseCase
) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<
        Results<Ok<GetAllDiagramEnvironmentsResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetDiagramEnvironments([FromRoute] Guid diagramId) =>
        await getAllUseCase.Execute(
            new GetAllDiagramEnvironmentsRequest { DiagramId = diagramId }
        ) switch
        {
            { IsSuccess: true, Value: var value } => TypedResults.Ok(value),
            { Error.Error: var error }
                => error switch
                {
                    GetAllDiagramEnvironmentsError.DiagramNotFound => TypedResults.NotFound(),
                    GetAllDiagramEnvironmentsError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };

    [Authorize]
    [HttpPost]
    public async Task<
        Results<Ok<CreateDiagramEnvironmentResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > CreateDiagramEnvironment(
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

    [Authorize]
    [HttpPut("{id}")]
    public async Task<
        Results<Ok<UpdateDiagramEnvironmentResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > UpdateDiagramEnvironment(
        [FromRoute] Guid diagramId,
        [FromRoute] Guid id,
        UpdateDiagramEnvironmentRequestDTO request
    ) =>
        await updateUseCase.Execute(
            new UpdateDiagramEnvironmentRequest
            {
                Id = id,
                IsActive = request.IsActive,
                ViewsCount = request.ViewsCount
            }
        ) switch
        {
            { IsSuccess: true, Value: var value } => TypedResults.Ok(value),
            { Error.Error: var error }
                => error switch
                {
                    UpdateDiagramEnvironmentError.NotFound => TypedResults.NotFound(),
                    UpdateDiagramEnvironmentError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };
}
