using Domain.Department.ValueObject;
using Domain.Shared;
using Domain.Shered;

namespace Domain.Department;

/**
 * <summary>
 * Представляет доменную сущность "Подразделение" (Department).
 * Описывает структуру организационной единицы, включая иерархические связи и метаданные.
 * </summary>
 */
public class DepartmentCs
{
    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="DepartmentCs"/>.
     * </summary>
     * <param name="id">Уникальный идентификатор подразделения.</param>
     * <param name="parentId">Идентификатор вышестоящего подразделения.</param>
     * <param name="name">Валидное наименование подразделения.</param>
     * <param name="identifier">Бизнес-идентификатор (код) подразделения.</param>
     * <param name="path">Строковое или иерархическое представление пути к подразделению.</param>
     * <param name="depth">Уровень вложенности в иерархии.</param>
     * <param name="lifeTime">Сведения о времени создания, обновления и статусе активности.</param>
     */
    public DepartmentCs(
        DepartmentId id,
        DepartmentId parentId,
        NotEmptyName name,
        DepartmentIdentifier identifier,
        DepartmentPath path,
        DepartmentDepth depth,
        EntityLifeTime lifeTime
    )
    {
        Id = id;
        ParentId = parentId;
        Name = name;
        Identifier = identifier;
        Path = path;
        Depth = depth;
        LifeTime = lifeTime;
    }

    /**
     * <summary>
     * Получает уникальный системный идентификатор подразделения.
     * </summary>
     */
    public DepartmentId Id { get; }

    /**
     * <summary>
     * Получает идентификатор родительского подразделения.
     * </summary>
     */
    public DepartmentId ParentId { get; }

    /**
     * <summary>
     * Получает наименование подразделения.
     * </summary>
     */
    public NotEmptyName Name { get; }

    /**
     * <summary>
     * Получает уникальный бизнес-код или строковый идентификатор подразделения.
     * </summary>
     */
    public DepartmentIdentifier Identifier { get; }

    /**
     * <summary>
     * Получает полный путь подразделения в структуре организации.
     * </summary>
     */
    public DepartmentPath Path { get; }

    /**
     * <summary>
     * Получает уровень глубины подразделения в дереве иерархии.
     * </summary>
     */
    public DepartmentDepth Depth { get; }

    /**
     * <summary>
     * Получает данные о жизненном цикле (создание, изменение, активность).
     * </summary>
     */
    public EntityLifeTime LifeTime { get; }
}
