using Domain.LocationContext.ValueObjects;
using Domain.Shared;
using Domain.Shered;

namespace Domain.LocationContext;

/**
 * <summary>
 * Представляет доменную сущность "Локация" (Location).
 * Описывает физическое местоположение объекта с учетом адреса и часового пояса.
 * </summary>
 */
public class Location
{
    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="Location"/>.
     * </summary>
     * <param name="id">Уникальный идентификатор локации.</param>
     * <param name="name">Валидное наименование локации.</param>
     * <param name="address">Физический адрес локации.</param>
     * <param name="lifeTime">Сведения о жизненном цикле (создание, активность).</param>
     * <param name="timeZone">Часовой пояс в формате IANA.</param>
     */
    public Location(
        LocationId id,
        NotEmptyName name,
        LocationAddress address,
        EntityLifeTime lifeTime,
        IanaTimeZone timeZone
    )
    {
        Id = id;
        Name = name;
        Address = address;
        LifeTime = lifeTime;
        TimeZone = timeZone;
    }

    /** <summary>Получает уникальный системный идентификатор локации.</summary> */
    public LocationId Id { get; }

    /** <summary>Получает наименование локации.</summary> */
    public NotEmptyName Name { get; }

    /** <summary>Получает физический адрес локации.</summary> */
    public LocationAddress Address { get; }

    /** <summary>Получает данные о жизненном цикле сущности.</summary> */
    public EntityLifeTime LifeTime { get; }

    /** <summary>Получает часовой пояс локации.</summary> */
    public IanaTimeZone TimeZone { get; }

    /**
     * <summary>
     * Фабричный метод для создания новой локации с автоматической генерацией ID и начального жизненного цикла.
     * </summary>
     * <param name="address">Объект-значение адреса.</param>
     * <param name="timeZone">Объект-значение часового пояса.</param>
     * <param name="name">Объект-значение имени.</param>
     * <returns>Новый экземпляр <see cref="Location"/>.</returns>
     */
    public static Location CreateNew(LocationAddress address, IanaTimeZone timeZone, NotEmptyName name)
    {
        /* Генерация нового уникального идентификатора */
        LocationId id = LocationId.CreateNew();

        /* Инициализация начальных временных меток жизненного цикла */
        EntityLifeTime lifeTime = EntityLifeTime.CreateInitial();

        /* Возвращаем новый объект, соблюдая порядок аргументов конструктора */
        return new Location(id, name, address, lifeTime, timeZone);
    }
}
