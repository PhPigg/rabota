using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.DepartmentContext;
using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext;
using System.Reflection;

namespace Infostructure;

public class Application_db_Context : DbContext
{
    private IOptions<db_connectionsoptions> options;
    public Application_db_Context(IOptions<db_connectionsoptions> opion)
    {
        options = opion;
    }
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<DepartmentPosition> DepartmentPositions => Set<DepartmentPosition>();
    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(options.Value.ToConnectionString());
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Регистрация Value Objects как owned types
        modelBuilder.Owned<DepartmentId>();
        modelBuilder.Owned<PositionId>();
        
        Assembly assembly = typeof(Application_db_Context).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }

}
