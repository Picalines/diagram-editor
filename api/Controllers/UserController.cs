using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using DiagramEditor.Controllers.Requests;
using DiagramEditor.Extensions;
using DiagramEditor.Repositories;
using DiagramEditor.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController(IUserRepository users, IAuthenticator auth) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetCurrent()
    {
        return auth.GetCurrentUser().MatchAction(Ok, BadRequest);
    }

    [Authorize]
    [HttpPut]
    public IActionResult UpdateCurrent()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody, Required] RegisterRequest register)
    {
        if (ModelState is { IsValid: false })
        {
            return BadRequest(ModelState);
        }

        return users
            .Create(register.Login, register.Password)
            .Map(auth.Authenticate)
            .Map(tokens => new { tokens.AccessToken, tokens.RefreshToken })
            .MapError(error => new { error })
            .MatchAction(Ok, BadRequest);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody, Required] LoginRequest login)
    {
        if (ModelState is { IsValid: false })
        {
            return BadRequest(ModelState);
        }

        return auth.IdentifyUser(login.Login, login.Password)
            .Map(auth.Authenticate)
            .Map(tokens => new { tokens.AccessToken, tokens.RefreshToken })
            .MatchAction(Ok, Unauthorized);
    }

    [Authorize]
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody, Required] RefreshRequest refresh)
    {
        if (ModelState is { IsValid: false })
        {
            return BadRequest(ModelState);
        }

        if (auth.GetCurrentUser().TryGetValue(out var user) is false)
        {
            return BadRequest();
        }

        return auth.Reauthenticate(user, refresh.RefreshToken)
            .Map(tokens => new { tokens.AccessToken, tokens.RefreshToken })
            .MatchAction(Ok, Forbid);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        throw new NotImplementedException();
    }
}
