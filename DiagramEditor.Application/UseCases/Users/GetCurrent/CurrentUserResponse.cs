using DiagramEditor.Application.DTOs;

namespace DiagramEditor.Application.UseCases.Users.GetCurrent;

public sealed record CurrentUserResponse(UserDTO User);
