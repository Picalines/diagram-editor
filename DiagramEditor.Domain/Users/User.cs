using System.Text.Json.Serialization;

namespace DiagramEditor.Domain.Users;

public sealed class User(string login, string passwordHash)
{
    public Guid Id { get; private set; }

    public string Login { get; set; } = login;

    [JsonIgnore]
    public string PasswordHash { get; set; } = passwordHash;

    public string DisplayName { get; set; } = login;

    public string? AvatarUrl { get; set; } = null;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public bool IsAdmin { get; set; } = false;
}
