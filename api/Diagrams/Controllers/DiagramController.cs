using DiagramEditor.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using DiagramEditor.Database.Models;
using DiagramEditor.Services.Authentication;
using DiagramEditor.Extensions;
using CSharpFunctionalExtensions;

namespace DiagramEditor.Controllers;

[ApiController]
[Route("diagram")]
public sealed class DiagramController(IDiagramRepository diagrams, IAuthenticator auth) : ControllerBase
{
    [Authorize]
    [HttpGet("{id}")]
    public Results<Ok<Diagram>, NotFound> GetDiagramById([FromRoute] int id)
    {
        if (auth.GetAuthenticatedUser().TryGetValue(out var user) is false)
        {
            return TypedResults.NotFound();
        }

        return diagrams.GetById(id)
            .Where(diagram => diagram.Creator.Id == user.Id)
            .ToTypedResult(TypedResults.Ok, TypedResults.NotFound);
    }
}
