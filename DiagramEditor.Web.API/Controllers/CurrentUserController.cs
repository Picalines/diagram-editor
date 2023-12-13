using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.UseCases.Users.GetCurrent;
using DiagramEditor.Application.UseCases.Users.UpdateCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user")]
public sealed class CurrentUserController(
    IGetCurrentUserUseCase getCurrentUseCase,
    IUpdateCurrentUserUseCase updateCurrentUseCase
) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult>> GetCurrentUser()
    {
        return await getCurrentUseCase.Execute(Unit.Instance) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    GetCurrentUserError.Unauthorized => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };
    }

    [Authorize]
    [HttpPut]
    public async Task<
        Results<
            Ok<UpdateCurrentUserResponse>,
            BadRequest<EnumError<UpdateCurrentUserError>?>,
            UnauthorizedHttpResult
        >
    > EditCurrentUser([FromBody, Required] UpdateCurrentUserRequest request)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest<EnumError<UpdateCurrentUserError>?>(null);
        }

        return await updateCurrentUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error: var error }
                => error.Error switch
                {
                    UpdateCurrentUserError.Unauthorized => TypedResults.Unauthorized(),
                    UpdateCurrentUserError.ValidationError
                    or UpdateCurrentUserError.LoginTaken
                        => TypedResults.BadRequest<EnumError<UpdateCurrentUserError>?>(error),
                    _ => throw new NotImplementedException(),
                },
        };
    }
}
