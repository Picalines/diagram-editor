using DiagramEditor.Application;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.UseCases.Diagrams.Create;
using DiagramEditor.Application.UseCases.Diagrams.Delete;
using DiagramEditor.Application.UseCases.Diagrams.GetInfo;
using DiagramEditor.Application.UseCases.Diagrams.GetOwned;
using DiagramEditor.Application.UseCases.Diagrams.UpdateInfo;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Web.API.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user/diagrams")]
public sealed class UserDiagramsController(
    IGetOwnedDiagramsUseCase getOwnedUseCase,
    ICreateDiagramUseCase createUseCase,
    IGetDiagramInfoUseCase getUseCase,
    IUpdateDiagramInfoUseCase updateUseCase,
    IDeleteDiagramUseCase deleteUseCase
) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<
        Results<Ok<GetOwnedDiagramsResponse>, UnauthorizedHttpResult>
    > GetOwnedDiagrams() =>
        await getOwnedUseCase.Execute(Unit.Instance) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    GetOwnedDiagramsError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                }
        };

    [Authorize]
    [HttpPost]
    public async Task<
        Results<Ok<CreateDiagramResponse>, BadRequest, UnauthorizedHttpResult>
    > CreateDiagram([FromBody] CreateDiagramRequest request) =>
        await createUseCase.Execute(request) switch
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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<
        Results<Ok<GetDiagramInfoResponse>, BadRequest, NotFound, UnauthorizedHttpResult>
    > GetDiagramById(Guid id) =>
        await getUseCase.Execute(
            new GetDiagramInfoRequest { Id = id, ViewMode = DiagramViewMode.InEditor }
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

    [Authorize]
    [HttpPut("{id}")]
    public async Task<
        Results<
            Ok<UpdateDiagramInfoResponse>,
            BadRequest<EnumError<UpdateDiagramInfoError>?>,
            NotFound,
            UnauthorizedHttpResult
        >
    > UpdateDiagramById(Guid id, UpdateDiagramInfoRequestDTO request) =>
        await updateUseCase.Execute(
            new UpdateDiagramInfoRequest
            {
                Id = id,
                Name = request.Name,
                Description = request.Description
            }
        ) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error: var error }
                => error.Error switch
                {
                    UpdateDiagramInfoError.Unauthorized => TypedResults.Unauthorized(),
                    UpdateDiagramInfoError.ValidationError
                        => TypedResults.BadRequest<EnumError<UpdateDiagramInfoError>?>(error),
                    UpdateDiagramInfoError.NotFound => TypedResults.NotFound(),
                    _ => throw new NotImplementedException(),
                },
        };

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<Results<Ok, BadRequest, NotFound, UnauthorizedHttpResult>> DeleteDiagram(
        Guid id
    ) =>
        await deleteUseCase.Execute(new DeleteDiagramRequest { Id = id }) switch
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
