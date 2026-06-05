using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;

namespace Domain.LocationContext.Repositories;

/**
 * <summary>
 * Интерфейс репозитория для работы с сущностью Location.
 * </summary>
 */
public interface ILocationRepository
{
    /**
     * <summary>
     * Добавляет новую локацию.
     * </summary>
     * <param name="location">Локация для добавления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task AddAsync(Location location, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Обновляет существующую локацию.
     * </summary>
     * <param name="location">Локация для обновления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task UpdateAsync(Location location, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Удаляет локацию по идентификатору.
     * </summary>
     * <param name="id">Идентификатор локации для удаления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    Task DeleteAsync(LocationId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Получает локацию по идентификатору.
     * </summary>
     * <param name="id">Идентификатор локации.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Локация или null, если не найдена.</returns>
     */
    Task<Location?> GetByIdAsync(LocationId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Получает все локации.
     * </summary>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Список всех локаций.</returns>
     */
    Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование локации по идентификатору.
     * </summary>
     * <param name="id">Идентификатор локации.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если локация существует.</returns>
     */
    Task<bool> ExistsAsync(LocationId id, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование локации по названию.
     * </summary>
     * <param name="name">Название локации.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если локация с таким названием существует.</returns>
     */
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование локации по названию (Value Object).
     * </summary>
     * <param name="name">Название локации (Value Object).</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если локация с таким названием существует.</returns>
     */
    Task<bool> ExistsByNameAsync(NotEmptyName name, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Проверяет существование локации по адресу (Value Object).
     * </summary>
     * <param name="address">Адрес локации (Value Object).</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>True, если локация с таким адресом существует.</returns>
     */
    Task<bool> ExistsByAddressAsync(LocationAddress address, CancellationToken cancellationToken = default);

    /**
     * <summary>
     * Удаляет локацию по идентификатору.
     * </summary>
     * <param name="id">Идентфикатор локации для удаления.</param>
     * <param name="cancellationToken">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     * <exception cref="InvalidOperationException">Выбрасывается, если локация не найдена.</exception>
     */
    Task DeleteAsync(LocationId id, CancellationToken cancellationToken = default);
}
