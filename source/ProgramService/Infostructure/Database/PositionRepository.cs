using Domain.PositionsContext;
using Domain.PositionsContext.Repositories;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infostructure.Database;

/**
 * <summary>
 * Реализация репозитория для работы с сущностью Position через EntityFramework.
 * </summary>
 */
public class PositionRepository : IPositionRepository
{
    private readonly Application_db_Context _context;

    public PositionRepository(Application_db_Context context)
    {
        _context = context;
    }

    public async Task AddAsync(Position position, CancellationToken cancellationToken = default)
    {
        await _context.Positions.AddAsync(position, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Position position, CancellationToken cancellationToken = default)
    {
        _context.Positions.Update(position);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(PositionId id, CancellationToken cancellationToken = default)
    {
        var position = await _context.Positions
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        if (position != null)
        {
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Position?> GetByIdAsync(PositionId id, CancellationToken cancellationToken = default)
    {
        return await _context.Positions
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Position>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Positions
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(PositionId id, CancellationToken cancellationToken = default)
    {
        return await _context.Positions
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Positions
            .AnyAsync(p => p.Name.Value == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(NotEmptyName name, CancellationToken cancellationToken = default)
    {
        return await _context.Positions
            .AnyAsync(p => p.Name.Value == name.Value, cancellationToken);
    }

    public async Task<IReadOnlyList<Position>> GetManyByIds(IEnumerable<PositionId> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Positions
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteManyAsync(IEnumerable<PositionId> ids, CancellationToken cancellationToken = default)
    {
        var idsList = ids.ToList();
        var positions = await _context.Positions
            .Where(p => idsList.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (positions.Count == 0)
        {
            throw new InvalidOperationException("Должности не найдены.");
        }

        _context.Positions.RemoveRange(positions);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
