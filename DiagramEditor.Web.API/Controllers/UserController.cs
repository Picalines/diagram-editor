using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using DiagramEditor.Application.UseCases.Users.GetCurrent;
using DiagramEditor.Application.UseCases.Users.Register;
using DiagramEditor.Application.UseCases.Users.UpdateCurrent;
using DiagramEditor.Application.UseCases;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("user")]
public sealed class UserController(
    IGetCurrentUserUseCase getCurrentUseCase,
    IUpdateCurrentUserUseCase updateCurrentUseCase,
    IRegisterUseCase registerUseCase
) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<Results<Ok<CurrentUserResponse>, UnauthorizedHttpResult>> GetCurrentUser()
    {
        return await getCurrentUseCase.Execute(Unit.Instance) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error } => error switch
            {
                GetCurrentUserError.Unauthorized => TypedResults.Unauthorized(),
                _ => throw new NotImplementedException(),
            },
        };
    }

    [Authorize]
    [HttpPut]
    public async Task<Results<
        Ok<UpdateCurrentUserResponse>,
        BadRequest<EnumError<UpdateCurrentUserError>?>,
        UnauthorizedHttpResult
    >> EditCurrentUser([FromBody, Required] UpdateCurrentUserRequest request)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest<EnumError<UpdateCurrentUserError>?>(null);
        }

        return await updateCurrentUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error: var error } => error.Error switch
            {
                UpdateCurrentUserError.Unauthorized => TypedResults.Unauthorized(),
                UpdateCurrentUserError.InvalidLogin
                or UpdateCurrentUserError.InvalidPassword
                or UpdateCurrentUserError.LoginTaken =>
                    TypedResults.BadRequest<EnumError<UpdateCurrentUserError>?>(error),
                _ => throw new NotImplementedException(),
            },
        };
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<Results<
        Ok<RegisterResponse>,
        BadRequest<EnumError<RegisterError>?>
    >> Register([FromBody, Required] RegisterRequest request)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest<EnumError<RegisterError>?>(null);
        }

        return await registerUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error: var error } => error.Error switch
            {
                RegisterError.InvalidLogin or RegisterError.InvalidPassword or RegisterError.LoginTaken =>
                    TypedResults.BadRequest<EnumError<RegisterError>?>(error),
                _ => throw new NotImplementedException(),
            },
        };
    }
}
