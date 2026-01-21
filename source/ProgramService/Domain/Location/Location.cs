using Domain.LocationContext.ValueObjects;
using Domain.Shered;

public class Location
{
    public Location(LocationId id, NotEmptyName name, LocationAddress address, EntityLifeTime lifeTime, IanaTimeZone timeZone)
    {
        Id = id;
        Name = name;
        Address = address;
        LifeTime = lifeTime;
        TimeZone = timeZone;
    }
    public LocationId Id { get; }
    public NotEmptyName Name { get; }
    public LocationAddress Address { get; }
    public EntityLifeTime LifeTime { get; }
    public IanaTimeZone TimeZone { get; }
}