using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using System.Net;
using static Domain.LocationContext.Location;

namespace Domain.PositionsContext;

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
    public Position(PositionId id, NotEmptyName name, PositionDescription description, EntityLifeTime lifeTime)
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

    public interface PositionNameUniquenessCriteria
    {
        bool IsSatisfiedBy(NotEmptyName Name);
    }

    public void ChangePositionName(PositionNameUniquenessCriteria criteria, NotEmptyName other)
    {
        if (!criteria.IsSatisfiedBy(other))
        {
            throw new InvalidOperationException("Название должности уже используется.");
        }

        Name = other;
    }
}
