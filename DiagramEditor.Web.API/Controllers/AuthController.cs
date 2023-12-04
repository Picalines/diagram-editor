using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain;
using DiagramEditor.Web.API.Controllers.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(IAuthenticator auth) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public Results<Ok<AuthTokens>, BadRequest, UnauthorizedHttpResult> Login([FromBody, Required] LoginRequest login)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        var tokens = auth.IdentifyUser(login.Login, login.Password)
            .Map(auth.Authenticate)
            .GetValueOrDefault();

        return tokens is { } ? TypedResults.Ok(tokens) : TypedResults.Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public Results<Ok<AuthTokens>, BadRequest, ForbidHttpResult> Refresh([FromBody, Required] RefreshRequest refresh)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        var authTokens = new AuthTokens(refresh.AccessToken, refresh.RefreshToken);

        if (auth.IdentifyUser(authTokens).TryGetValue(out var user) is false)
        {
            return TypedResults.BadRequest();
        }

        var tokens = auth.Reauthenticate(user, refresh.RefreshToken)
            .GetValueOrDefault();

        return tokens is { } ? TypedResults.Ok(tokens) : TypedResults.Forbid();
    }

    [Authorize]
    [HttpPost("logout")]
    public Results<Ok, BadRequest> Logout()
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        if (auth.GetAuthenticatedUser().TryGetValue(out var user) is false)
        {

            return TypedResults.BadRequest();
        }

        auth.Deauthenticate(user);
        return TypedResults.Ok();
    }
}
