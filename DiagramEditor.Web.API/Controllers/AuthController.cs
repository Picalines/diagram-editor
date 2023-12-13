using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application;
using DiagramEditor.Application.UseCases.Authentication.Login;
using DiagramEditor.Application.UseCases.Authentication.Logout;
using DiagramEditor.Application.UseCases.Authentication.Refresh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(
    ILoginUseCase loginUseCase,
    IRefreshUseCase refreshUseCase,
    ILogoutUseCase logoutUseCase
) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<Results<Ok<LoginResponse>, BadRequest, UnauthorizedHttpResult>> Login(
        [FromBody, Required] LoginRequest request
    ) =>
        await loginUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    LoginError.InvalidCredentials => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<Results<Ok<RefreshResponse>, BadRequest, ForbidHttpResult>> Refresh(
        [FromBody, Required] RefreshRequest request
    ) =>
        await refreshUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error.Error: var error }
                => error switch
                {
                    RefreshError.InvalidCredentials => TypedResults.Forbid(),
                    RefreshError.RefreshExpired => TypedResults.Forbid(),
                    _ => throw new NotImplementedException(),
                },
        };

    [Authorize]
    [HttpPost("logout")]
    public async Task<Results<Ok, BadRequest, UnauthorizedHttpResult>> Logout() =>
        await logoutUseCase.Execute(Unit.Instance) switch
        {
            { IsSuccess: true } => TypedResults.Ok(),
            { Error.Error: var error }
                => error switch
                {
                    LogoutError.NotAuthenticated => TypedResults.Unauthorized(),
                    _ => throw new NotImplementedException(),
                },
        };
}
