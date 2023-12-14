namespace DiagramEditor.Domain.Users;

public sealed class User
{
    public Guid Id { get; private set; }

    public required string Login { get; set; }

    public required string PasswordHash { get; set; }

    public required string DisplayName { get; set; }

    public string? AvatarUrl { get; set; } = null;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public bool IsAdmin { get; set; } = false;
}
