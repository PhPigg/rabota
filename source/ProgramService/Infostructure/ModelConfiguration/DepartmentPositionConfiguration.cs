using Domain.DepartmentContext;
using Domain.DepartmentContext.ValueObject;
using Domain.PositionsContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infostructure.ModelConfiguration;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("DepartmentPositions");
    
        // Составной первичный ключ
        builder.HasKey(x => new { x.DepartmentId, x.PositionId });
        
        // Конфигурация свойств-идентификаторов
        builder.Property(x => x.DepartmentId).HasColumnName("DepartmentId").HasConversion(x => x.Value, y => DepartmentId.Create(y));
        builder.Property(x => x.PositionId).HasColumnName("PositionId").HasConversion(x => x.Value, y => PositionId.Create(y));
        
        // Конфигурация отношений
        builder.HasOne(x => x.Department)
            .WithMany(x => x.Positions)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Position)
            .WithMany()
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
