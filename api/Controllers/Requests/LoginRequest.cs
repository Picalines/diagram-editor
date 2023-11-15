using System.ComponentModel.DataAnnotations;

namespace DiagramEditor.Controllers.Requests;

public sealed record LoginRequest([Required] string Login, [Required] string Password);
