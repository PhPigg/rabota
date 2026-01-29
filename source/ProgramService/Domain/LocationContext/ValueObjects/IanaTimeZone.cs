namespace Domain.LocationContext.ValueObjects;

/**
 * <summary>
 * Представляет часовой пояс в формате IANA (например, "Europe/Moscow").
 * Обеспечивает базовую валидацию структуры идентификатора временной зоны.
 * </summary>
 */
public record IanaTimeZone
{
    /** <summary>Возвращает строковое значение часового пояса.</summary> */
    public string Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="IanaTimeZone"/>.
     * </summary>
     * <param name="value">Валидная строка часового пояса.</param>
     */
    private IanaTimeZone(string value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="IanaTimeZone"/> с проверкой формата IANA.
     * </summary>
     * <param name="value">Идентификатор часового пояса (например, "Asia/Almaty").</param>
     * <returns>Новый объект <see cref="IanaTimeZone"/>.</returns>
     * <exception cref="ArgumentNullException">Выбрасывается, если строка пуста.</exception>
     * <exception cref="ArgumentException">Выбрасывается, если формат не соответствует структуре "Область/Город".</exception>
     */
    public static IanaTimeZone Create(string value)
    {
        /* Проверка на пустую строку */
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Значение часового пояса не может быть пустым.");
        }

        /* Проверка на наличие разделителя */
        if (!value.Contains('/', StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Значение введено некорректно. Ожидается формат 'Область/Город'.",
                nameof(value)
            );
        }

        /* Разделение строки и проверка количества сегментов */
        string[] parts = value.Split('/');

        /* Стандартные IANA зоны обычно состоят из 2 частей (реже 3, но здесь ограничимся базовой проверкой) */
        if (parts.Length != 2)
        {
            throw new ArgumentException(
                "Значение некорректно. Идентификатор должен состоять из двух сегментов.",
                nameof(value)
            );
        }

        /* Проверка, что ни один сегмент не является пустым */
        if (parts.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Сегменты часового пояса не могут быть пустыми.", nameof(value));
        }

        return new IanaTimeZone(value);
    }
}
