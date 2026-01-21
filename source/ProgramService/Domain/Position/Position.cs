using Domain.LocationContext.ValueObjects;
using Domain.Position.ValueObjects;
using Domain.Shered;
using System.Net;

public class Position
{
    public Position(PositionId id, NotEmptyName name, PositionDescreption descreption, EntityLifeTime lifeTime)
    {
        Id = id;
        Name = name;
        Descreption = descreption;
        LifeTime = lifeTime;
    }
    public PositionId Id { get; }
    public NotEmptyName Name { get; }
    public PositionDescreption Descreption { get; }
    public EntityLifeTime LifeTime { get; }
}