using DiagramEditor.Attributes;
using DiagramEditor.Configuration;
using DiagramEditor.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DiagramEditor.Database;

[Injectable(ServiceLifetime.Transient)]
public sealed class ApplicationContext(IConfiguration configuration) : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Diagram> Diagrams { get; set; } = null!;

    public DbSet<AdminNote> AdminNotes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var mySqlConfiguration =
            configuration.GetSection("MySql").Get<MySqlConfiguration>()
            ?? throw new InvalidOperationException("invalid MySql configuration");

        var mySqlConnectionString = CreateConnectionString(
            ("server", mySqlConfiguration.Server),
            ("user", mySqlConfiguration.User),
            ("password", mySqlConfiguration.Password),
            ("database", mySqlConfiguration.Database)
        );

        var mySqlVersion = new MySqlServerVersion(mySqlConfiguration.Version);

        options.UseMySql(mySqlConnectionString, mySqlVersion);
    }

    private static string CreateConnectionString(params (string Key, string Value)[] parameters)
    {
        return string.Concat(parameters.Select(pair => $"{pair.Key}={pair.Value};"));
    }
}
