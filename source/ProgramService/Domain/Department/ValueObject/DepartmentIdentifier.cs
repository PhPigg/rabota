namespace Domain.Department.ValueObject;

/**
 * <summary>
 * Представляет уникальный бизнес-идентификатор (псевдоним) подразделения.
 * Гарантирует, что значение состоит только из латинских символов.
 * </summary>
 */
public record DepartmentIdentifier
{
    /** <summary>Возвращает строковое значение псевдонима.</summary> */
    public string Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="DepartmentIdentifier"/>.
     * </summary>
     * <param name="value">Валидное латинское значение.</param>
     */
    private DepartmentIdentifier(string value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="DepartmentIdentifier"/> с валидацией символов.
     * </summary>
     * <param name="value">Строка, содержащая только латинские буквы.</param>
     * <returns>Новый объект <see cref="DepartmentIdentifier"/>.</returns>
     * <exception cref="ArgumentException">Выбрасывается, если строка пуста или содержит нелатинские символы.</exception>
     */
    public static DepartmentIdentifier Create(string value)
    {
        /* Проверка на пустую строку или пробелы */
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Название псевдонима не может быть пустым.", nameof(value));
        }

        /* Исправленная логика: проверяем, что каждый символ является латинской буквой.
           Используем char.IsBetween или стандартную проверку диапазонов A-Z и a-z.
        */
        bool isOnlyLatin = value.All(x => (x >= 'A' && x <= 'Z') || (x >= 'a' && x <= 'z'));

        if (!isOnlyLatin)
        {
            throw new ArgumentException("Название псевдонима должно содержать только латинские буквы.", nameof(value));
        }

        return new DepartmentIdentifier(value);
    }
}
