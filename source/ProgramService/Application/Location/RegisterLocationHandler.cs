using Domain.LocationContext;
using Domain.LocationContext.Repositories;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;
using DomainLocation = Domain.LocationContext.Location;

namespace Application.Location;

public sealed class RegisterLocationHandler
{
    private readonly ILocationRepository _repository;
    private readonly ILocationUniquenessCriteria _criteria;

    public RegisterLocationHandler(
        ILocationRepository repository,
        ILocationUniquenessCriteria criteria)
    {
        _repository = repository;
        _criteria = criteria;
    }

    public async Task<Guid> Handle(
        RegisterLocationCommand command,
        CancellationToken ct = default)
    {
        ValidateLocationNames(command);
        var location = CreateLocation(command);
        await _repository.AddAsync(location, ct);
        return location.Id.Value;
    }

    private void ValidateLocationNames(RegisterLocationCommand command)
    {
        IReadOnlyList<string> names = command.LocationNames;
        if (names.Count == 0)
        {
            return;
        }
        const int maxLength = 255;
        foreach (string name in names)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Название локации не может быть пустым.");
            }
            if (name.Length > maxLength)
            {
                throw new InvalidOperationException($"Название локации слишком длинное ({name.Length} символов). Максимально допустимое количество символов: {maxLength}.");
            }
        }
    }

    private DomainLocation CreateLocation(RegisterLocationCommand command)
    {
        // Создаем value objects из строковых данных команды
        var name = NotEmptyName.Create(command.LocationNames.First());
        var address = LocationAddress.Create(command.Address);
        var timeZone = IanaTimeZone.Create(command.IanaTimeZone);
        
        // Используем статический фабричный метод сущности
        var location = DomainLocation.CreateNew(_criteria, address, timeZone, name);
        
        return location;
    }
}
