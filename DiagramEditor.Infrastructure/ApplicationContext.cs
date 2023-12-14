using DiagramEditor.Application.Attributes;
using DiagramEditor.Domain.Diagrams;
using DiagramEditor.Domain.Users;
using DiagramEditor.Infrastructure.Configuration.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure;

[Injectable(ServiceLifetime.Transient)]
public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Diagram> Diagrams { get; set; } = null!;

    public DbSet<DiagramElement> DiagramElements { get; set; } = null!;

    public DbSet<DiagramElementProperty> DiagramElementProperties { get; set; } = null!;

    public DbSet<DiagramEnvironment> DiagramEnvironments { get; set; } = null!;

    private static MySqlConfigurationSection _mySqlConfiguration = null!;

    public ApplicationContext(MySqlConfigurationSection mySqlConfiguration)
    {
        if (_mySqlConfiguration is null)
        {
            _mySqlConfiguration = mySqlConfiguration;

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
