using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.Shared;
using System.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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
    public DepartmentId? ParentId { get; private set; }

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

    
    //список должностей в подразделении
    private readonly List<DepartmentPosition> _positions;
    public IReadOnlyList<DepartmentPosition> Positions => _positions.AsReadOnly();
    
    //список локаций в подразделении
    private readonly List<DepartmentLocation> _locations;
    public IReadOnlyList<DepartmentLocation> Locations => _locations.AsReadOnly();

    

    //метод для изменения названия подразделения с учетом уникальности
    public void ChangeDepartmentName(DepartmentUniqueeCriteria criteria, NotEmptyName other)
    {

        CheckForActive();
        if (!criteria.IsSatisfiedBy(other))
        {
            throw new ArgumentException("Название локации уже существует.");
        }
        Name = other;
        //обновление даты редактирования
        UpDateTimeEdit();
    }

    //метод создания подразделения с учетом уникальности
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

    //метод для создания и возвращения иерархического пути для переданного подразделения
    //путь создается путем объединения имен подразделений через разделитель
    private DepartmentPath CreateHierarchicalPath(Department department)
    {
        //разделитель между именами подразделений
        const char separator = '/';

        //обьъединяет имена подразделений через разделитель
        string[] names = [Name.Value, department.Name.Value];
        string joinedName = string.Join(separator, names);
        return DepartmentPath.Create(joinedName);
    }

    //рассчитывает и возвращает уровень иерархии для переданного подразделения
    private HierarchyLevel CalculateHierarchyLevel(Department department)


    //метод для привязки подразделения к другому подразделению
    public void ConnectDepartment(Department department)
    {
        if (IsSameDepartment(department))
        {
            throw new InvalidOperationException("Подразделение не может быть родителем самого себя");
        }
        
        if (IsDescendantOf(department))
        {
            throw new InvalidOperationException("Подразделение не может быть привязано к своему потомку");
        }
    }

    //метод для добавления локации в список локаций подразделения
    public void AddLocation(Location location)
    {
        CheckForActive();
        
        //проверяем существующие DepartmentLocation
        foreach (DepartmentLocation existing in Locations)
        {
            if (existing.Location.Name == location.Name)
            {
                throw new ArgumentException("Локация с таким названием уже существует в данном подразделении");
            }
            
            if (existing.Location.Address == location.Address)
            {
                throw new ArgumentException("Локация с таким адресом уже существует в данном подразделении");
            }

            if (existing.Location.Id == location.Id)
            {
                throw new ArgumentException("Локация с таким идентификатором уже существует в данном подразделении");
            }
        }
        _locations.Add(new DepartmentLocation(this, location));
        UpDateTimeEdit();
    }

    //метод для добавления должности в список должностей подразделения
    public void AddPosition(Position position)
    {
        CheckForActive();

        // Проверяем существующие DepartmentPosition
        foreach (DepartmentPosition existing in Positions)
        {
            if (existing.Position.Name == position.Name)
            {
                throw new ArgumentException("Должность с таким названием уже существует в данном подразделении");
            }

            if (existing.Position.Id == position.Id)
            {
                throw new ArgumentException("Должность с таким идентификатором уже существует в данном подразделении");
            }
        }
        _positions.Add(new DepartmentPosition(this, position));
        UpDateTimeEdit();
    }

    //метод для проверки, является ли переданное подразделение тем же самым подразделением
    private bool IsSameDepartment(Department department)
    {
        return Id == department.Id;
    }

    //метод для проверки, является ли текущее подразделение потомком переданного подразделения
    private bool IsDescendantOf(Department department)
    {
        if (department.ParentId == null)
        {
            return false;
        }
        return department.ParentId == Id;
    }

    //метод для проверки активности сущности
    private void CheckForActive()
    {
        if (LifeTime.IsActive == false)
        {
            throw new ArgumentException("Сущность удалена");
        }
    }

    //метод для обновления даты обновления
    private void UpDateTimeEdit()
    {
        LifeTime = LifeTime.Update();
    }
}
