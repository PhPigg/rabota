namespace Domain.Department.ValueObject;

/**
 * <summary>
 * Представляет уникальный идентификатор подразделения (Value Object).
 * Обеспечивает строгую типизацию и гарантирует валидность системного идентификатора.
 * </summary>
 */
public class DepartmentId
{
    /** <summary>Возвращает внутреннее значение GUID.</summary> */
    public Guid Value { get; }

    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="DepartmentId"/>.
     * </summary>
     * <param name="value">Валидный уникальный идентификатор.</param>
     */
    private DepartmentId(Guid value)
    {
        Value = value;
    }

    /**
     * <summary>
     * Создает экземпляр <see cref="DepartmentId"/> с проверкой на пустое значение.
     * </summary>
     * <param name="value">Значение идентификатора типа <see cref="Guid"/>.</param>
     * <returns>Новый объект <see cref="DepartmentId"/>.</returns>
     * <exception cref="ArgumentException">Выбрасывается, если передан пустой Guid (Empty).</exception>
     */
    public static DepartmentId Create(Guid value)
    {
        /* Проверка, что Guid не является дефолтным (нулевым) */
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор подразделения не может быть пустым.", nameof(value));
        }

        return new DepartmentId(value);
    }
}
