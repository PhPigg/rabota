using Domain.Department.ValueObject;
using Domain.LocationContext.ValueObjects;
using Domain.Position.ValueObjects;
using Domain.Shered;
using System.Net;

public class Department
{
    public Department(DepartmentId id, DepartmentId parentid, NotEmptyName name, DepartmentIdentifier identifier, DepartmentPath path, DepartmentDepth depth, EntityLifeTime lifeTime)
    {
        Id = id;
        ParentId = parentid;
        Name = name;
        Depth = depth;
        Identifier = identifier;
        Path = path;
        LifeTime = lifeTime;
    }
    public DepartmentId Id { get; }
    public DepartmentId ParentId { get; }
    public NotEmptyName Name { get; }
    public DepartmentIdentifier Identifier { get; }
    public DepartmentPath Path { get; }
    public DepartmentDepth Depth { get; }
    public EntityLifeTime LifeTime { get; }
}