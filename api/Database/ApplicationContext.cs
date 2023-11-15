using DiagramEditor.Attributes;
using DiagramEditor.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DiagramEditor.Database;

[Injectable(ServiceLifetime.Transient)]
public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Diagram> Diagrams { get; set; } = null!;

    public DbSet<AdminNote> AdminNotes { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(
            "server=localhost;user=root;password=123456;database=app;",
            new MySqlServerVersion(new Version(8, 1, 0))
        );
    }
}
