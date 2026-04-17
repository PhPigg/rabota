using System.Collections.Concurrent;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using static Domain.PositionsContext.ValueObjects.PositionDescription;

namespace Domain.InMemory;

public static class InMemoryPositionRepository
{
    private static readonly ConcurrentDictionary<PositionId, Position> _positions = new();

    public static void Add(Position position, IPositionNameUniquenessCriteria criteria)
    {
        if (_positions.ContainsKey(position.Id))
        {
            throw new ArgumentException("Должность с таким идентификатором уже существует.");
        }

        if (!criteria.IsSatisfiedBy(position.Name))
        {
            throw new ArgumentException("Должность с таким названием уже существует.");
        }

        _positions[position.Id] = position;
    }

    public static bool Remove(PositionId id)
    {
        return _positions.TryRemove(id, out _);
    }

    public static bool HardRemove(PositionId id)
    {
        if (_positions.TryGetValue(id, out var position))
        {
            // Полное удаление из хранилища
            _positions.TryRemove(id, out _);
            return true;
        }
        return false;
    }

    public static Position? GetById(PositionId id)
    {
        _positions.TryGetValue(id, out var position);
        return position;
    }

    public static IReadOnlyList<Position> GetAll()
    {
        return _positions.Values.ToList().AsReadOnly();
    }

    public static bool Exists(PositionId id)
    {
        return _positions.ContainsKey(id);
    }

    public static void InitializeSeedData(IPositionNameUniquenessCriteria criteria)
    {
        if (_positions.Any())
        {
            return;
        }

        var positions = new[]
        {
            Position.CreateNew(criteria, 
                NotEmptyName.Create("Разработчик"), 
                PositionDescription.Create("Разработка и поддержка программного обеспечения")),
            
            Position.CreateNew(criteria, 
                NotEmptyName.Create("Старший разработчик"), 
                PositionDescription.Create("Разработка архитектуры и менторство")),
            
            Position.CreateNew(criteria, 
                NotEmptyName.Create("Аналитик"), 
                PositionDescription.Create("Анализ требований и проектирование систем")),
            
            Position.CreateNew(criteria, 
                NotEmptyName.Create("Менеджер проекта"), 
                PositionDescription.Create("Управление проектами и командами"))
        };

        foreach (var position in positions)
        {
            _positions[position.Id] = position;
        }
    }
}
