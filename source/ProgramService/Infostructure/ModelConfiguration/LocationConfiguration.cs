using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infostructure.ModelConfiguration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").HasConversion(x => x.Value, y => LocationId.Create(y));
        builder.Property(x => x.Name).HasColumnName("Name").HasConversion(x => x.Value, y => NotEmptyName.Create(y));
        builder.Property(x => x.Address).HasColumnName("Address").HasConversion(x => x.Value, y => LocationAddress.Create(y));
        builder.Property(x => x.TimeZone).HasColumnName("TimeZone").HasConversion(x => x.Value, y => IanaTimeZone.Create(y));
        builder.ComplexProperty(x => x.LifeTime, y => {
            y.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
            y.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt").IsRequired(false);
            y.Property(a => a.DeletedAt).HasColumnName("DeletedAt").IsRequired(false);
        });
    

        throw new NotImplementedException();
    }
    
}
