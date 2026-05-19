using Domain.DepartmentContext;
using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infostructure.ModelConfiguration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Depth).HasColumnName("Depth").HasConversion(x => x.Value, y => DepartmentDepth.Create(y));
        builder.Property(x => x.Id).HasColumnName("Id").HasConversion(x => x.Value, y => DepartmentId.Create(y));
        builder.Property(x => x.Identifier).HasColumnName("Identifier").HasConversion(x => x.Value, y => DepartmentIdentifier.Create(y));
        builder.Property(x => x.Path).HasColumnName("Path").HasConversion(x => x.Value, y => DepartmentPath.Create(y));
        builder.Property(x => x.Name).HasColumnName("Name").HasConversion(x => x.Value, y => NotEmptyName.Create(y));
        builder.ComplexProperty(x => x.LifeTime, y =>
        {
            y.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
            y.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            y.Property(a => a.DeletedAt).HasColumnName("DeletedAt").IsRequired(false);
        });
        builder.HasMany<DepartmentLocation>(x => x.Locations)
            .WithOne(x => x.Department)
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany<DepartmentPosition>(x => x.Positions)
            .WithOne(x => x.Department)
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
