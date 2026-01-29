namespace Domain.Shared;

/**
 * <summary>
 * Представляет собой объект-значение (Value Object) для работы с непустыми именами.
 * Инкапсулирует логику валидации длины и содержания.
 * </summary>
 */
public class NotEmptyName
{
    /** <summary>Минимально допустимая длина имени.</summary> */
    public const int MinLength = 3;

    /** <summary>Максимально допустимая длина имени (исправлено с MaxLenght).</summary> */
    public const int MaxLength = 128;

    /** <summary>Строковое значение имени.</summary> */
    public string Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="NotEmptyName"/>.
     * </summary>
     * <param name="value">Проверенное строковое значение.</param>
     */
    private NotEmptyName(string value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="NotEmptyName"/> после прохождения всех проверок.
     * </summary>
     * <param name="value">Входящая строка для валидации.</param>
     * <returns>Экземпляр <see cref="NotEmptyName"/>.</returns>
     * <exception cref="ArgumentException">
     * Генерируется, если строка пуста, состоит из пробелов или не попадает в диапазон от 3 до 128 символов.
     * </exception>
     */
    public static NotEmptyName Create(string value)
    {
        /* Проверка на null, пустую строку или строку из одних пробелов */
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Название не может быть пустым.", nameof(value));
        }

        /* Проверка на превышение лимита символов */
        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Название не может превышать {MaxLength} символов.", nameof(value));
        }

        /* Проверка на соответствие минимальным требованиям длины */
        if (value.Length < MinLength)
        {
            throw new ArgumentException($"Название должно быть от {MinLength} до {MaxLength} символов.", nameof(value));
        }

        return new NotEmptyName(value);
    }
}
