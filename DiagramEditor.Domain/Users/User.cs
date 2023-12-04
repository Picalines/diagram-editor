using CSharpFunctionalExtensions;

namespace DiagramEditor.Domain.Users;

public readonly record struct UserId(Guid Value);

public sealed class User
{
    public UserId Id { get; private set; }

    public required string Login { get; set; }

    public required string PasswordHash { get; set; }

    public required string DisplayName { get; set; }

    public Maybe<string> AvatarUrl { get; set; } = Maybe.None;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsAdmin { get; set; } = false;
}
