using Domain.DepartmentContext;
using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using System.Net;
using static Domain.LocationContext.Location;

namespace Domain.PositionsContext;

//интерфейс для активности сущности
public interface ILifeTimeable
{
    EntityLifeTime LifeTime { get; set; }
}

//интерфейс для уникальности названия
public interface IPositionNameUniquenessCriteria
{
    bool IsSatisfiedBy(NotEmptyName Name);
}

/**
 * <summary>
 * Представляет доменную сущность "Должность" (Position).
 * Объединяет идентификатор, наименование, описание и данные о жизненном цикле.
 * </summary>
 */
public class Position
{
    /**
     * <summary>
     * Инициализирует новый экземпляр класса <see cref="Position"/>.
     * </summary>
     * <param name="id">Уникальный идентификатор должности.</param>
     * <param name="name">Валидное наименование должности.</param>
     * <param name="description">Детальное описание обязанностей или требований.</param>
     * <param name="lifeTime">Информация о времени создания и статусе активности.</param>
     */
    private Position(PositionId id, NotEmptyName name, PositionDescription description, EntityLifeTime lifeTime)
    {
        Id = id;
        Name = name;
        Description = description;
        LifeTime = lifeTime;
    }

    /**
     * <summary>
     * Получает уникальный идентификатор данной должности.
     * </summary>
     */
    public PositionId Id { get; set; }

    /**
     * <summary>
     * Получает объект-значение, содержащий название должности.
     * </summary>
     */
    public NotEmptyName Name { get; set; }

    /**
     * <summary>
     * Получает описание должности (функциональные обязанности, требования).
     * </summary>
     */
    public PositionDescription Description { get; set; }

    /**
     * <summary>
     * Получает информацию о временных метках и состоянии сущности.
     * </summary>
     */
    public EntityLifeTime LifeTime { get; set; }

    

    //метод для проверки уникальности названия
    public void ChangePositionName(IPositionNameUniquenessCriteria criteria, NotEmptyName other)
    {
        ThrowIfNotActive();   
        if (!criteria.IsSatisfiedBy(other))
        {
            throw new InvalidOperationException("Название должности уже используется.");
        }

        Name = other;
        UpDateTimeEdit();
    }

    public static Position CreateNew(IPositionNameUniquenessCriteria criteria, NotEmptyName name, PositionDescription description)
    {
        //Проверка названия подразделения на уникальность
        if (!criteria.IsSatisfiedBy(name))
        {
            throw new ArgumentException("Название должности уже существует.");
        }

        /* Генерация нового уникального идентификатора */
        PositionId id = PositionId.CreateNew();

        /* Инициализация начальных временных меток жизненного цикла */
        EntityLifeTime lifeTime = EntityLifeTime.CreateInitial();

        /* Возвращаем новый объект, соблюдая порядок аргументов конструктора */
        return new Position(id, name, description, lifeTime);
    }

    

    private void UpDateTimeEdit()
    {
        LifeTime = LifeTime.Update();
    }
}
