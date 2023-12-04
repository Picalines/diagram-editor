using DiagramEditor.Domain.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagramEditor.Infrastructure.Configuration.Database.Entities;

internal sealed class DiagramEntityConfiguration : IEntityTypeConfiguration<Diagram>
{
    public void Configure(EntityTypeBuilder<Diagram> builder)
    {
        builder.HasKey(diagram => diagram.Id);
        builder.Property(diagram => diagram.Id).HasConversion(
            diagramId => diagramId.Value,
            value => new DiagramId(value)
        );
    }
}
