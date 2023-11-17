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
        return auth.GetCurrentUser().MatchAction(Ok, Unauthorized);
    }

    [Authorize]
    [HttpPut]
    public IActionResult UpdateCurrent()
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest register)
    {
        if (ModelState is { IsValid: false })
        {
            return BadRequest(ModelState);
        }

        return users
            .Create(register.Login, register.Password)
            .MatchAction(Ok, creationError => BadRequest(new { reason = creationError }));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        if (ModelState is { IsValid: false })
        {
            return BadRequest(ModelState);
        }

        return auth.Identify(login.Login, login.Password)
            .Bind(user => auth.Authenticate(user))
            .MatchAction(
                token =>
                    Ok(new { accessToken = token.AccessToken, refreshToken = token.RefreshToken }),
                Unauthorized
            );
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        throw new NotImplementedException();
    }
}
