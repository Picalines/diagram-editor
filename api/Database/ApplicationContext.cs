using DiagramEditor.Attributes;
using DiagramEditor.Configuration;
using DiagramEditor.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DiagramEditor.Database;

[Injectable(ServiceLifetime.Transient)]
public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Diagram> Diagrams { get; set; } = null!;

    public DbSet<AdminNote> AdminNotes { get; set; } = null!;

    private static MySqlConfiguration _mySqlConfiguration = null!;

    public ApplicationContext(IConfiguration configuration)
    {
        if (_mySqlConfiguration is null)
        {
            _mySqlConfiguration =
                configuration.Get<MySqlConfiguration>()
                ?? throw new InvalidOperationException("invalid MySql configuration");

            Database.EnsureCreated();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var mySqlConnectionString = CreateConnectionString(
            ("server", _mySqlConfiguration.Server),
            ("uid", "root"),
            ("pwd", _mySqlConfiguration.RootPassword),
            ("database", _mySqlConfiguration.Database)
        );

        var mySqlVersion = new MySqlServerVersion(_mySqlConfiguration.Version);

        options.UseMySql(mySqlConnectionString, mySqlVersion);
    }

    private static string CreateConnectionString(params (string Key, string Value)[] parameters)
    {
        return string.Join(";", parameters.Select(pair => $"{pair.Key}={pair.Value}"));
    }
}
