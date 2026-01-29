namespace Domain.Department.ValueObject;

/**
 * <summary>
 * Представляет иерархический путь подразделения (например, "Головной офис/IT/Разработка").
 * Используется для визуализации структуры и навигации по дереву отделов.
 * </summary>
 */
public record DepartmentPath
{
    /** <summary>Возвращает строковое представление пути.</summary> */
    public string Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="DepartmentPath"/>.
     * </summary>
     * <param name="value">Валидная строка пути.</param>
     */
    private DepartmentPath(string value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр пути после проверки входных данных.
     * </summary>
     * <param name="value">Строка, представляющая путь подразделения.</param>
     * <returns>Новый объект <see cref="DepartmentPath"/>.</returns>
     * <exception cref="ArgumentException">Выбрасывается, если строка пуста или содержит только пробелы.</exception>
     */
    public static DepartmentPath Create(string value)
    {
        /* Проверка на пустую строку или наличие только пробельных символов */
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Путь не может быть пустым или состоять из пробелов.", nameof(value));
        }

        return new DepartmentPath(value);
    }
}
