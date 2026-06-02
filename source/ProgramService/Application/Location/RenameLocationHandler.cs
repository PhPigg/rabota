using Domain.LocationContext;
using Domain.LocationContext.Repositories;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;

namespace Application.Location;

public sealed class RenameLocationHandler
{
    private readonly ILocationRepository _repository;
    private readonly ILocationUniquenessCriteria _criteria;

    public RenameLocationHandler(
        ILocationRepository repository,
        ILocationUniquenessCriteria criteria)
    {
        _repository = repository;
        _criteria = criteria;
    }

    public async Task<Guid> Handle(
        RenameLocationCommand command,
        CancellationToken ct = default)
    {
        var locationId = LocationId.Create(command.Id);
        
        Domain.LocationContext.Location? location = await _repository.GetByIdAsync(locationId, ct);
        if (location is null)
        {
            string message = $"Локация с идентификатором {command.Id} не найдена";
            throw new InvalidOperationException(message);
        }

        NotEmptyName name = NotEmptyName.Create(command.NewName);
        
        // Проверяем уникальность нового имени
        location.ChangeLocationName(_criteria, name);
        
        await _repository.UpdateAsync(location, ct);
        
        return location.Id.Value;
    }
}
