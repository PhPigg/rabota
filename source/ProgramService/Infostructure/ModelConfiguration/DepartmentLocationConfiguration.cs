using Domain.DepartmentContext;
using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext.ValueObjects;
using Domain.LocationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infostructure.ModelConfiguration;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("DepartmentLocations");
        // Составной первичный ключ
        builder.HasKey(x => new { x.DepartmentId, x.LocationId });
        // Конфигурация свойств-идентификаторов
        builder.Property(x => x.DepartmentId).HasColumnName("DepartmentId").HasConversion(x => x.Value, y => DepartmentId.Create(y));
        builder.Property(x => x.LocationId).HasColumnName("LocationId").HasConversion(x => x.Value, y => LocationId.Create(y));
    }
}
