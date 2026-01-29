namespace Domain.Shared;

/**
 * <summary>
 * Представляет неизменяемую структуру данных для отслеживания временных меток жизненного цикла сущности.
 * </summary>
 */
public sealed record EntityLifeTime
{
    /**
     * <summary>
     * Получает дату и время создания сущности.
     * </summary>
     */
    public DateTime CreatedAt { get; }

    /**
     * <summary>
     * Получает дату и время последнего обновления сущности.
     * </summary>
     */
    public DateTime UpdatedAt { get; }

    /**
     * <summary>
     * Получает значение, указывающее, считается ли сущность активной.
     * </summary>
     */
    public bool IsActive { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="EntityLifeTime"/>.
     * </summary>
     * <param name="createdAt">Временная метка создания.</param>
     * <param name="updatedAt">Временная метка последнего изменения.</param>
     * <param name="isActive">Текущий статус активности.</param>
     */
    private EntityLifeTime(DateTime createdAt, DateTime updatedAt, bool isActive)
    {
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        IsActive = isActive;
    }

    /**
     * <summary>
     * Создает и валидирует объект <see cref="EntityLifeTime"/>.
     * </summary>
     * <param name="createdAt">Дата создания (не может быть MinValue или MaxValue).</param>
     * <param name="updatedAt">Дата обновления (не может быть меньше даты создания).</param>
     * <param name="isActive">Флаг активности.</param>
     * <returns>Валидный экземпляр <see cref="EntityLifeTime"/>.</returns>
     * <exception cref="ArgumentException">
     * Выбрасывается, если переданы некорректные даты или нарушена хронология.
     * </exception>
     */
    public static EntityLifeTime Create(DateTime createdAt, DateTime updatedAt, bool isActive)
    {
        /* Проверка на допустимый диапазон дат для создания */
        if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
        {
            throw new ArgumentException("Некорректное значение даты создания.", nameof(createdAt));
        }

        /* Проверка на допустимый диапазон дат для обновления */
        if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
        {
            throw new ArgumentException("Некорректное значение даты обновления.", nameof(updatedAt));
        }

        /* Проверка бизнес-логики: обновление не может произойти раньше создания */
        if (updatedAt < createdAt)
        {
            throw new ArgumentException("Дата обновления не может быть меньше даты создания.", nameof(updatedAt));
        }

        return new EntityLifeTime(createdAt, updatedAt, isActive);
    }

    /**
     * <summary>
     * Создает начальное состояние жизненного цикла для новой сущности.
     * </summary>
     * <returns>Экземпляр с текущим временем в качестве даты создания/обновления и статусом IsActive = true.</returns>
     */
    public static EntityLifeTime CreateInitial()
    {
        /* Используем UtcNow для предотвращения проблем с часовыми поясами */
        DateTime now = DateTime.UtcNow;
        return new EntityLifeTime(now, now, true);
    }
}
