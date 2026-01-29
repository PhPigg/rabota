namespace Domain.PositionsContext.ValueObjects;

/**
 * <summary>
 * Представляет уникальный идентификатор позиции (Value Object).
 * Гарантирует, что идентификатор всегда является валидным и не пустым.
 * </summary>
 */
public record PositionId
{
    /** <summary>Возвращает внутреннее значение GUID.</summary> */
    public Guid Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="PositionId"/>.
     * </summary>
     * <param name="value">Уникальный идентификатор.</param>
     */
    private PositionId(Guid value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Фабричный метод для создания <see cref="PositionId"/> с валидацией.
     * </summary>
     * <param name="value">Значение типа <see cref="Guid"/>.</param>
     * <returns>Экземпляр <see cref="PositionId"/>.</returns>
     * <exception cref="ArgumentException">Выбрасывается, если передан пустой GUID.</exception>
     */
    public static PositionId Create(Guid value)
    {
        /* Проверка на пустой идентификатор (00000000-0000-0000-0000-000000000000) */
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор не может быть пустым.", nameof(value));
        }

        return new PositionId(value);
    }
}
