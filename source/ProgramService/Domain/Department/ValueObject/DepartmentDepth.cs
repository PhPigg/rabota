namespace Domain.Department.ValueObject;

/**
 * <summary>
 * Представляет уровень вложенности подразделения в иерархии (Value Object).
 * </summary>
 */
public record DepartmentDepth
{
    /** <summary>Возвращает числовое значение глубины вложенности.</summary> */
    public short Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="DepartmentDepth"/>.
     * </summary>
     * <param name="value">Валидное значение уровня вложенности.</param>
     */
    private DepartmentDepth(short value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="DepartmentDepth"/> с проверкой диапазона.
     * </summary>
     * <param name="value">Уровень вложенности (должен быть 1 или выше).</param>
     * <returns>Новый объект <see cref="DepartmentDepth"/>.</returns>
     * <exception cref="ArgumentException">Выбрасывается, если значение меньше 1.</exception>
     */
    public static DepartmentDepth Create(short value)
    {
        /* Проверка: иерархия начинается с 1 (например, корень — 1, дочерние — 2 и т.д.) */
        if (value < 1)
        {
            throw new ArgumentException("Глубина должна быть положительным числом больше нуля.", nameof(value));
        }

        return new DepartmentDepth(value);
    }
}
