using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;

namespace Domain.PositionsContext.Repositories;

/**
 * <summary>
 * Интерфейс репозитория для работы с сущностью Position.
 * </summary>
 */
public interface IPositionRepository
{
    /**
     * <summary>
     * Добавляет новую должность.
     * </summary>
     * <param name="position">Должность для добавления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task AddAsync(Position position, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Обновляет существующую должность.
     * </summary>
     * <param name="position">Должность для обновления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task UpdateAsync(Position position, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Удаляет должность по идентификатору.
     * </summary>
     * <param name="id">Идентификатор должности для удаления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task DeleteAsync(PositionId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Получает должность по идентификатору.
     * </summary>
     * <param name="id">Идентификатор должности.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Должность или null, если не найдена.</returns>
     */
    Task<Position?> GetByIdAsync(PositionId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Получает все должности.
     * </summary>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Список всех должностей.</returns>
     */
    Task<IReadOnlyList<Position>> GetAllAsync(CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование должности по идентификатору.
     * </summary>
     * <param name="id">Идентификатор должности.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если должность существует.</returns>
     */
    Task<bool> ExistsAsync(PositionId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование должности по названию.
     * </summary>
     * <param name="name">Название должности.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если должность с таким названием существует.</returns>
     */
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование должности по названию (Value Object).
     * </summary>
     * <param name="name">Название должности (Value Object).</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если должность с таким названием существует.</returns>
     */
    Task<bool> ExistsByNameAsync(NotEmptyName name, CancellationToken cancellationToken = default);
}
