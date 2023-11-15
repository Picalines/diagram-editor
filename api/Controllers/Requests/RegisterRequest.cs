using System.ComponentModel.DataAnnotations;

namespace DiagramEditor.Controllers.Requests;

public sealed record RegisterRequest([Required] string Login, [Required] string Password);
