using DiagramEditor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagramEditor.Infrastructure.Configuration.Database.Entities;

internal sealed class DiagramEnvironmentEntityConfiguration : IEntityTypeConfiguration<DiagramEnvironment>
{
    public void Configure(EntityTypeBuilder<DiagramEnvironment> builder)
    {
        builder.HasKey(env => env.Id);
        builder.Property(env => env.Id).HasConversion(
            envId => envId.Value,
            value => new DiagramEnvironmentId(value)
        );
    }
}
