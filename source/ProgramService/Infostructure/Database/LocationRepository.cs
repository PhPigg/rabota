using Domain.LocationContext;
using Domain.LocationContext.Repositories;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infostructure.Database;

/**
 * <summary>
 * Реализация репозитория для работы с сущностью Location через EntityFramework.
 * </summary>
 */
public class LocationRepository : ILocationRepository
{
    private readonly Application_db_Context _context;

    public LocationRepository(Application_db_Context context)
    {
        _context = context;
    }

    public async Task AddAsync(Location location, CancellationToken cancellationToken = default)
    {
        await _context.Locations.AddAsync(location, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Location location, CancellationToken cancellationToken = default)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(LocationId id, CancellationToken cancellationToken = default)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        
        if (location == null)
        {
            throw new InvalidOperationException("Локация не найдена.");
        }

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Location?> GetByIdAsync(LocationId id, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(LocationId id, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AnyAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AnyAsync(l => l.Name.Value == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(NotEmptyName name, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AnyAsync(l => l.Name.Value == name.Value, cancellationToken);
    }

    public async Task<bool> ExistsByAddressAsync(string address, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AnyAsync(l => l.Address.Value == address, cancellationToken);
    }

    public async Task<bool> ExistsByAddressAsync(LocationAddress address, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AnyAsync(l => l.Address.Value == address.Value, cancellationToken);
    }
}
