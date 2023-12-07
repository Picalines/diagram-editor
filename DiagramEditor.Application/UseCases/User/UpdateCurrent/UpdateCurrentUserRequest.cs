namespace DiagramEditor.Application.UseCases.User.UpdateCurrent;

public sealed record UpdateCurrentUserRequest : IRequest
{
    public string? Login { get; init; }

    public string? Password { get; init; }

    public string? DisplayName { get; init; }

    public string? AvatarUrl { get; init; }
}
