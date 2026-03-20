using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.Shared;
using System.Data;
using System.Net;

namespace Domain.DepartmentContext;

//интерфейс для уникальности названия подразделения
public interface DepartmentUniqueeCriteria
{
    bool IsSatisfiedBy(NotEmptyName name);
}

/**
 * <summary>
 * Представляет доменную сущность "Подразделение" (Department).
 * Описывает структуру организационной единицы, включая иерархические связи и метаданные.
 * </summary>
 */
public class Department
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
    private Department(
        DepartmentId id,
        DepartmentId parentId,
        NotEmptyName name,
        DepartmentIdentifier identifier,
        DepartmentPath path,
        DepartmentDepth depth,
        EntityLifeTime lifeTime,
        IEnumerable<DepartmentLocation>? Locations = null,
        IEnumerable<DepartmentPosition>? Positions = null
    )
    {
        Id = id;
        ParentId = parentId;
        Name = name;
        Identifier = identifier;
        Path = path;
        Depth = depth;
        LifeTime = lifeTime;
        Locations = Locations is null ? [] : [.. Locations];
        Positions = Positions is null ? [] : [.. Positions];
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
    public NotEmptyName Name { get; set; }

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
    
    public EntityLifeTime LifeTime { get; set; }

    //***************************************************
    private readonly List<DepartmentPosition> _positions;
    //***************************************************
    private readonly List<DepartmentLocation> _locations;
    //****************************************************************************
    public IReadOnlyList<DepartmentPosition> Positions => _positions.AsReadOnly();
    //****************************************************************************
    public IReadOnlyList<DepartmentLocation> Locations => _locations.AsReadOnly();

    //метод для создания названия подразделения с учетом уникальности
    public void ChangeDepartmentName(DepartmentUniqueeCriteria criteria, NotEmptyName other)
    {

        CheckForActive();
        if (!criteria.IsSatisfiedBy(other))
        {
            throw new ArgumentException("Название локации уже существует.");
        }
        //обновление даты редактирования
        UpDateTimeEdit();
        Name = other;
    }

    public static Department CreateNew(DepartmentUniqueeCriteria criteria, DepartmentId id,
        DepartmentId parentId,
        NotEmptyName name,
        DepartmentIdentifier identifier,
        DepartmentPath path,
        DepartmentDepth depth,
        EntityLifeTime lifeTime,
        IEnumerable<DepartmentLocation>? Locations = null,
        IEnumerable<DepartmentPosition>? Positions = null)
    {
        //Проверка названия подразделения на уникальность
        if (!criteria.IsSatisfiedBy(name))
        {
            throw new ArgumentException("Название локации уже существует.");
        }

        


        /* Возвращаем новый объект, соблюдая порядок аргументов конструктора */
        return new Department(id, parentId, name, identifier, path, depth, lifeTime, Locations, Positions);
    }



    public void AddLocation(Location location)
    {
        CheckForActive();
        
        foreach (Location existing in Locations)
        {
            if (existing.Name == location.Name)
            {
                throw new ArgumentException("Локация с таким названием уже существует в данном подразделении");
            }
            
            if (existing.Address == location.Address)
            {
                throw new ArgumentException("Локация с таким адресом уже существует в данном подразделении");
            }

            if (existing.Id == location.Id)
            {
                throw new ArgumentException("Локация с таким идентификатором уже существует в данном подразделении");
            }
        }
        UpDateTimeEdit();
        _locations.Add(location);
    }

    public void AddPosition(Position position)
    {
        CheckForActive();
        
        foreach (Position existing in Positions)
        {
            if (existing.Name == position.Name)
            {
                throw new ArgumentException("Должность с таким названием уже существует в данном подразделении");
            }

            

            if (existing.Id == position.Id)
            {
                throw new ArgumentException("Должность с таким идентификатором уже существует в данном подразделении");
            }
        }

        UpDateTimeEdit();
        _positions.Add(position);
    }

    private void CheckForActive()
    {
        if (LifeTime.IsActive == false)
        {
            throw new ArgumentException("Сущность удалена");
        }
    }

    private void UpDateTimeEdit()
    {
        LifeTime = LifeTime.Update();
    }
}
