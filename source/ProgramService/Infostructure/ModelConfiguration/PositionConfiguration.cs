using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.DepartmentContext;

namespace Infostructure.ModelConfiguration;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("Positions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").HasConversion(x => x.Value, y => PositionId.Create(y));
        builder.Property(x => x.Name).HasColumnName("Name").HasConversion(x => x.Value, y => NotEmptyName.Create(y));
        builder.Property(x => x.Description).HasColumnName("Description").HasConversion(x => x.Value, y => PositionDescription.Create(y));
        builder.ComplexProperty(x => x.LifeTime, y =>
        {
            y.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
            y.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            y.Property(a => a.DeletedAt).HasColumnName("DeletedAt").IsRequired(false);
        });
        builder.HasMany<DepartmentPosition>(x => x.Departments)
            .WithOne(x => x.Position)
            .HasForeignKey(x => x.PositionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
