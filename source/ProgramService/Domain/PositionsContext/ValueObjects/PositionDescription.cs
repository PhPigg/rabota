namespace Domain.PositionsContext.ValueObjects;

/**
 * <summary>
 * Представляет описание должности.
 * Гарантирует, что строка не является пустой и не превышает допустимый лимит символов.
 * </summary>
 */
public record PositionDescription
{
    /** <summary>Максимально допустимая длина описания.</summary> */
    public const int MaxLength = 500;

    /** <summary>Строковое значение описания.</summary> */
    public string Value { get; }

    /**
     * <summary>
     * Приватный конструктор для создания объекта-значения.
     * </summary>
     * <param name="value">Валидное значение описания.</param>
     */
    private PositionDescription(string value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="PositionDescription"/> с валидацией входных данных.
     * </summary>
     * <param name="value">Текст описания должности.</param>
     * <returns>Объект <see cref="PositionDescription"/>.</returns>
     * <exception cref="ArgumentNullException">Выбрасывается, если строка пуста или состоит из пробелов.</exception>
     * <exception cref="ArgumentException">Выбрасывается, если длина текста превышает 500 символов.</exception>
     */
    public static PositionDescription Create(string value)
    {
        /* Проверка на наличие контента в строке */
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Значение описания не может быть пустым.");
        }

        /* Проверка на соблюдение лимита длины */
        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Описание превышает допустимую длину в {MaxLength} символов.", nameof(value));
        }

        return new PositionDescription(value);
    }
}
