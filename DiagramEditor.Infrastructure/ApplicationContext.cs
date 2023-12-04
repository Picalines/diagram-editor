using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;
using DiagramEditor.Infrastructure.Configuration.Database;
using Microsoft.EntityFrameworkCore;

namespace DiagramEditor.Infrastructure;

internal sealed class ApplicationContext(MySqlConfigurationSection mySqlConfiguration) : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Diagram> Diagrams { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var mySqlConnectionString = CreateConnectionString(
            ("server", mySqlConfiguration.Server),
            ("uid", "root"),
            ("pwd", mySqlConfiguration.RootPassword),
            ("database", mySqlConfiguration.Database)
        );

        var mySqlVersion = new MySqlServerVersion(mySqlConfiguration.Version);

        options.UseMySql(mySqlConnectionString, mySqlVersion);
    }

    private static string CreateConnectionString(params (string Key, string Value)[] parameters)
    {
        return string.Join(";", parameters.Select(pair => $"{pair.Key}={pair.Value}"));
    }
}
