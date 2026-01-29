namespace Domain.LocationContext.ValueObjects;

/**
 * <summary>
 * Представляет физический адрес локации, разделенный на составные части.
 * Обеспечивает форматирование адреса через запятую и предоставляет доступ к отдельным компонентам.
 * </summary>
 */
public record LocationAddress
{
    /** <summary>Внутренний список частей адреса.</summary> */
    private readonly List<string> _addressParts;

    /** <summary>Полное строковое представление адреса.</summary> */
    public string Value { get; }

    /** <summary>Список отдельных компонентов адреса (город, улица и т.д.) только для чтения.</summary> */
    public IReadOnlyList<string> AddressParts => _addressParts.AsReadOnly();

    /**
     * <summary>
     * Приватный конструктор для инициализации объекта-значения.
     * </summary>
     * <param name="parts">Коллекция очищенных частей адреса.</param>
     */
    private LocationAddress(IEnumerable<string> parts)
    {
        _addressParts = [.. parts];
        Value = string.Join(", ", _addressParts);
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="LocationAddress"/> на основе строки, разделенной запятыми.
     * </summary>
     * <param name="value">Полная строка адреса.</param>
     * <returns>Новый экземпляр <see cref="LocationAddress"/>.</returns>
     * <exception cref="ArgumentNullException">Выбрасывается, если входящая строка пуста.</exception>
     * <exception cref="ArgumentException">Выбрасывается, если после разделения строки не найдено валидных частей.</exception>
     */
    public static LocationAddress Create(string value)
    {
        /* Проверка на пустую входную строку */
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Адрес локации не может быть пустым.");
        }

        /* Разделение строки по запятой, удаление лишних пробелов и фильтрация пустых элементов */
        List<string> parts =
        [
            .. value.Split(',').Select(part => part.Trim()).Where(part => !string.IsNullOrWhiteSpace(part)),
        ];

        /* Проверка, что в адресе есть хотя бы один содержательный компонент */
        if (parts.Count == 0)
        {
            throw new ArgumentException(
                "Адрес локации должен содержать хотя бы одну значимую часть (например, город или улицу).",
                nameof(value)
            );
        }

        return new LocationAddress(parts);
    }
}
