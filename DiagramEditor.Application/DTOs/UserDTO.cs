using DiagramEditor.Domain.Users;

namespace DiagramEditor.Application.DTOs;

public sealed record UserDTO
{
    public required Guid Id { get; init; }

    public required string Login { get; set; }

    public required string DisplayName { get; set; }

    public string? AvatarUrl { get; set; } = null;

    public static UserDTO FromUser(User user) =>
        new UserDTO
        {
            Id = user.Id,
            Login = user.Login,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
        };
}
