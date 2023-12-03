using System.Text.Json.Serialization;

namespace DiagramEditor.Database.Models;

public sealed class User(string login, string passwordHash)
{
    public int Id { get; set; }

    public string Login { get; set; } = login;

    [JsonIgnore]
    public string PasswordHash { get; set; } = passwordHash;

    public string DisplayName { get; set; } = login;

    public string? AvatarUrl { get; set; } = null;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public bool IsAdmin { get; set; } = false;

    [JsonIgnore]
    public ISet<Diagram> Diagrams { get; } =
        new HashSet<Diagram>(ReferenceEqualityComparer.Instance);

    [JsonIgnore]
    public ISet<DiagramAccess> Accesses { get; } =
        new HashSet<DiagramAccess>(ReferenceEqualityComparer.Instance);

    [JsonIgnore]
    public ISet<AdminNote> AdminNotes { get; } =
        new HashSet<AdminNote>(ReferenceEqualityComparer.Instance);
}
