using System.ComponentModel.DataAnnotations;
using DiagramEditor.Application.Errors;
using DiagramEditor.Application.UseCases.Users.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersController(IRegisterUseCase registerUseCase) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<
        Results<Ok<RegisterResponse>, BadRequest<EnumError<RegisterError>?>>
    > Register([FromBody, Required] RegisterRequest request)
    {
        if (ModelState is { IsValid: false })
        {
            return TypedResults.BadRequest<EnumError<RegisterError>?>(null);
        }

        return await registerUseCase.Execute(request) switch
        {
            { IsSuccess: true, Value: var response } => TypedResults.Ok(response),
            { Error: var error }
                => error.Error switch
                {
                    RegisterError.ValidationError
                    or RegisterError.LoginTaken
                        => TypedResults.BadRequest<EnumError<RegisterError>?>(error),
                    _ => throw new NotImplementedException(),
                },
        };
    }
}
