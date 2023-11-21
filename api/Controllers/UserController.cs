using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using DiagramEditor.Controllers.Requests;
using DiagramEditor.Database.Models;
using DiagramEditor.Extensions;
using DiagramEditor.Repositories;
using DiagramEditor.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Controllers;

using DiagramEditor.Controllers.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

[ApiController]
[Route("user")]
public sealed class UserController(IUserRepository users, IAuthenticator auth) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public Results<Ok<User>, BadRequest> GetCurrent()
    {
        return auth.GetAuthenticatedUser().ToTypedResult(TypedResults.Ok, TypedResults.BadRequest);
    }

    [Authorize]
    [HttpPut]
    public IActionResult PutCurrent()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public Results<Ok<AuthTokensResponse>, BadRequest<string>> Register(
        [FromBody, Required] RegisterRequest register
    )
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest("invalid body");
        }

        return users
            .Create(register.Login, register.Password)
            .Map(auth.Authenticate)
            .Map(AuthTokensResponse.FromTuple)
            .MapError(error => error.ToString())
            .ToTypedResult(TypedResults.Ok, TypedResults.BadRequest);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public Results<Ok<AuthTokensResponse>, BadRequest, UnauthorizedHttpResult> Login(
        [FromBody, Required] LoginRequest login
    )
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        var tokens = auth.IdentifyUser(login.Login, login.Password)
            .Map(auth.Authenticate)
            .Map(AuthTokensResponse.FromTuple)
            .GetValueOrDefault();

        return tokens is { } ? TypedResults.Ok(tokens) : TypedResults.Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public Results<Ok<AuthTokensResponse>, BadRequest, ForbidHttpResult> Refresh(
        [FromBody, Required] RefreshRequest refresh
    )
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest();
        }

        if (auth.GetUserByAccessToken(refresh.AccessToken).TryGetValue(out var user) is false)
        {
            return TypedResults.BadRequest();
        }

        var tokens = auth.Reauthenticate(user, refresh.RefreshToken)
            .Map(AuthTokensResponse.FromTuple)
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
