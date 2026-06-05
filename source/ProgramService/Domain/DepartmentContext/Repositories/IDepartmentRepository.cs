using Domain.DepartmentContext;
using Domain.DepartmentContext.ValueObject;

namespace Domain.DepartmentContext.Repositories;

/**
 * <summary>
 * Интерфейс репозитория для работы с сущностью Department.
 * </summary>
 */
public interface IDepartmentRepository
{
    /**
     * <summary>
     * Добавляет новое подразделение.
     * </summary>
     * <param name="department">Подразделение для добавления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task AddAsync(Department department, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Обновляет существующее подразделение.
     * </summary>
     * <param name="department">Подразделение для обновления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task UpdateAsync(Department department, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Удаляет подразделение по идентификатору.
     * </summary>
     * <param name="id">Идентификатор подразделения для удаления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     * <exception cref="InvalidOperationException">Выбрасывается, если подразделение не найдено.</exception>
     */
    Task DeleteAsync(DepartmentId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Получает подразделение по идентификатору.
     * </summary>
     * <param name="id">Идентификатор подразделения.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Подразделение или null, если не найдено.</returns>
     */
    Task<Department?> GetByIdAsync(DepartmentId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Получает все подразделения.
     * </summary>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Список всех подразделений.</returns>
     */
    Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование подразделения по идентификатору.
     * </summary>
     * <param name="id">Идентификатор подразделения.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если подразделение существует.</returns>
     */
    Task<bool> ExistsAsync(DepartmentId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование подразделения по названию.
     * </summary>
     * <param name="name">Название подразделения.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если подразделение с таким названием существует.</returns>
     */
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
