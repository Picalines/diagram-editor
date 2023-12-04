using DiagramEditor.Domain.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagramEditor.Infrastructure.Configuration.Database.Entities;

internal sealed class DiagramElementEntityConfiguration : IEntityTypeConfiguration<DiagramElement>
{
    public void Configure(EntityTypeBuilder<DiagramElement> builder)
    {
        builder.HasKey(element => element.Id);
        builder.Property(element => element.Id).HasConversion(
            elementId => elementId.Value,
            value => new DiagramElementId(value)
        );
    }
}
