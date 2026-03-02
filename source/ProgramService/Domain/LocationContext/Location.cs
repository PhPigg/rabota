using Domain.LocationContext.ValueObjects;
using Domain.Shared;

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
    public NotEmptyName Name { get; set; }

    /** <summary>Получает физический адрес локации.</summary> */
    public LocationAddress Address { get; set; }

    /** <summary>Получает данные о жизненном цикле сущности.</summary> */
    public EntityLifeTime LifeTime { get; set; }

    /** <summary>Получает часовой пояс локации.</summary> */
    public IanaTimeZone TimeZone { get; set; }

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
    
    
    //изменение название локации, IANA, адреса локации
    

   

    public interface LocationAddressUniquenessCriteria
    {
        bool IsSatisfiedBy(LocationAddress Address);
    }

    public interface LocationNameUniquenessCriteria
    {
        bool IsSatisfiedBy(NotEmptyName Name);
    }

    //метод изменения региона
    public void ChangeIanaTimeZone(IanaTimeZone other)
    {
        TimeZone = other;
    }

    //метод изменения имени локации
    public void ChangeLocationName(LocationNameUniquenessCriteria criteria, NotEmptyName other)
    {
        if (!criteria.IsSatisfiedBy(other))
        {
            throw new InvalidOperationException("Название локации уже используется.");
        }

        Name = other;
    }

    //метод изменения адреса локации
    public void ChangeLocationAddress(LocationAddressUniquenessCriteria criteria, LocationAddress other)
    {
        if (!criteria.IsSatisfiedBy(other))
        {
            throw new InvalidOperationException("Адрес локации уже используется.");
        }

        Address = other;
    }

}
