using DiagramEditor.Application.DTOs;

namespace DiagramEditor.Application.UseCases.Users.Register;

public sealed record RegisterResponse
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }

    public required UserDTO User { get; init; }
}
