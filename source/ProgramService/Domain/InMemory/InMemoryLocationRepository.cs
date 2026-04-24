using System.Collections.Concurrent;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;
using static Domain.LocationContext.ValueObjects.LocationAddress;

namespace Domain.InMemory;

public static class InMemoryLocationRepository
{
    private static readonly ConcurrentDictionary<LocationId, Location> _locations = new();

    public static void Add(Location location)
    {
        if (_locations.ContainsKey(location.Id))
        {
            throw new ArgumentException("Локация с таким идентификатором уже существует.");
        }

        

        _locations[location.Id] = location;
    }

    public static bool Remove(LocationId id)
    {
        return _locations.TryRemove(id, out _);
    }

    

    public static Location? GetById(LocationId id)
    {
        _locations.TryGetValue(id, out var location);
        return location;
    }

    public static IReadOnlyList<Location> GetAll()
    {
        return _locations.Values.ToList().AsReadOnly();
    }

    public static bool Exists(LocationId id)
    {
        return _locations.ContainsKey(id);
    }

    public static void InitializeSeedData(ILocationUniquenessCriteria criteria)
    {
        if (_locations.Any())
        {
            return;
        }

        var locations = new[]
        {
            Location.CreateNew(criteria, 
                LocationAddress.Create("г. Москва, ул. Тверская, д. 1"), 
                IanaTimeZone.Create("Europe/Moscow"), 
                NotEmptyName.Create("Главный офис")),
            
            Location.CreateNew(criteria, 
                LocationAddress.Create("г. Санкт-Петербург, Невский проспект, д. 10"), 
                IanaTimeZone.Create("Europe/Moscow"), 
                NotEmptyName.Create("Северный офис")),
            
            Location.CreateNew(criteria, 
                LocationAddress.Create("г. Казань, ул. Баумана, д. 5"), 
                IanaTimeZone.Create("Europe/Moscow"), 
                NotEmptyName.Create("Офис Казань"))
        };

        foreach (var location in locations)
        {
            _locations[location.Id] = location;
        }
    }
}
