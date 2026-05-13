using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.PositionsContext;
using Domain.DepartmentContext;
using Domain.LocationContext;

namespace Infostructure;

public class Application_db_Context : DbContext
{
    private IOptions<db_connectionsoptions> options;
    public Application_db_Context(IOptions<db_connectionsoptions> opion)
    {
        options = opion;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseNpgsql(options.Value.ToConnectionString());
    }

}
