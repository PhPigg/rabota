using Domain.DepartmentContext;
using Domain.DepartmentContext.Repositories;
using Domain.DepartmentContext.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace Infostructure.Database;

/**
 * <summary>
 * Реализация репозитория для работы с сущностью Department через EntityFramework.
 * </summary>
 */
public class DepartmentRepository : IDepartmentRepository
{
    private readonly Application_db_Context _context;

    public DepartmentRepository(Application_db_Context context)
    {
        _context = context;
    }

    public async Task AddAsync(Department department, CancellationToken cancellationToken = default)
    {
        await _context.Departments.AddAsync(department, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(DepartmentId id, CancellationToken cancellationToken = default)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        
        if (department == null)
        {
            throw new InvalidOperationException("Отдел не найден.");
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Department?> GetByIdAsync(DepartmentId id, CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(DepartmentId id, CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .AnyAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Departments
            .AnyAsync(d => d.Name.Value == name, cancellationToken);
    }
}
