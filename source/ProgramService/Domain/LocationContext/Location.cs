using Domain.LocationContext.ValueObjects;

public class Location
{
    public Location(LocationId id, LocationName name, LocationAddress address, EntityLifeTime lifeTime, IanaTimeZone timeZone)
    {
        Id = id;
        Name = name;
        Address = address;
        LifeTime = lifeTime;
        TimeZone = timeZone;
    }
    public LocationId Id { get; }
    public LocationName Name { get; }
    public LocationAddress Address { get; }
    public EntityLifeTime LifeTime { get; }
    public IanaTimeZone TimeZone { get; }
}