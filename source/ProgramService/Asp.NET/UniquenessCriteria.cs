using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;

namespace Asp.NET;

/// <summary>
/// Реализация критерия уникальности для локаций.
/// Проверяет уникальность названия и адреса локации в репозитории.
/// </summary>
public class LocationUniquenessCriteria : ILocationUniquenessCriteria
{
    public bool IsSatisfiedBy(NotEmptyName name)
    {
        var existing = Domain.InMemory.InMemoryLocationRepository.GetAll();
        return !existing.Any(l => l.Id != LocationId.CreateNew() && l.Name.Value == name.Value);
    }

    public bool IsSatisfiedBy(LocationAddress address)
    {
        var existing = Domain.InMemory.InMemoryLocationRepository.GetAll();
        return !existing.Any(l => l.Id != LocationId.CreateNew() && l.Address.Value == address.Value);
    }
}

/// <summary>
/// Реализация критерия уникальности для должностей.
/// Проверяет уникальность названия должности в репозитории.
/// </summary>
public class PositionNameUniquenessCriteria : IPositionNameUniquenessCriteria
{
    public bool IsSatisfiedBy(NotEmptyName name)
    {
        var existing = Domain.InMemory.InMemoryPositionRepository.GetAll();
        return !existing.Any(p => p.Id != PositionId.CreateNew() && p.Name.Value == name.Value);
    }
}
