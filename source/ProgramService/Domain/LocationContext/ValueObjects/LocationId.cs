namespace Domain.LocationContext.ValueObjects;

/**
 * <summary>
 * Представляет уникальный идентификатор локации (Value Object).
 * Гарантирует целостность ссылки на сущность Location.
 * </summary>
 */
public class LocationId
{
    /** <summary>Возвращает внутреннее значение Guid.</summary> */
    public Guid Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="LocationId"/>.
     * </summary>
     * <param name="value">Значение идентификатора.</param>
     */
    private LocationId(Guid value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="LocationId"/> на основе существующего Guid с валидацией.
     * </summary>
     * <param name="value">Уникальный идентификатор.</param>
     * <returns>Объект <see cref="LocationId"/>.</returns>
     * <exception cref="ArgumentException">Выбрасывается, если передан пустой Guid.</exception>
     */
    public static LocationId Create(Guid value)
    {
        /* Проверка на пустой идентификатор */
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор локации не может быть пустым.", nameof(value));
        }

        return new LocationId(value);
    }

    /**
     * <summary>
     * Генерирует новый уникальный идентификатор для новой локации.
     * </summary>
     * <returns>Новый экземпляр <see cref="LocationId"/> с уникальным Guid.</returns>
     */
    public static LocationId CreateNew()
    {
        /* Создаем новый объект с использованием системного генератора Guid */
        return new LocationId(Guid.NewGuid());
    }
}
