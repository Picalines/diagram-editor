using DiagramEditor.Domain.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagramEditor.Infrastructure;

internal sealed class DiagramElementPropertyEntityConfiguration : IEntityTypeConfiguration<DiagramElementProperty>
{
    public void Configure(EntityTypeBuilder<DiagramElementProperty> builder)
    {
        builder.HasKey(property => property.Id);
        builder.Property(property => property.Id).HasConversion(
            propertyId => propertyId.Value,
            value => new DiagramElementPropertyId(value)
        );
    }
}
