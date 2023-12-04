using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Domain;
using DiagramEditor.Domain.Users;
using DiagramEditor.Web.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

using CSharpFunctionalExtensions;
using DiagramEditor.Web.API.Controllers.Requests;

[ApiController]
[Route("user")]
public sealed class UserController(IUserRepository users, IAuthenticator auth) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public Results<Ok<User>, BadRequest> GetCurrentUser()
    {
        return auth
            .GetAuthenticatedUser()
            .ToTypedResult(TypedResults.Ok, TypedResults.BadRequest);
    }

    [Authorize]
    [HttpPut]
    public IActionResult EditCurrentUser()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public Results<Ok<AuthTokens>, BadRequest<string>> Register([FromBody, Required] RegisterRequest register)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest("invalid body");
        }

        return users
            .Register(register.Login, register.Password)
            .Map(auth.Authenticate)
            .MapError(error => error.ToString())
            .ToTypedResult(TypedResults.Ok, TypedResults.BadRequest);
    }
}
