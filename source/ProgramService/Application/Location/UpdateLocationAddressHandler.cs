using Domain.LocationContext;
using Domain.LocationContext.Repositories;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;

namespace Application.Location;

public sealed class UpdateLocationAddressHandler
{
    private readonly ILocationRepository _repository;
    private readonly ILocationUniquenessCriteria _criteria;

    public UpdateLocationAddressHandler(
        ILocationRepository repository,
        ILocationUniquenessCriteria criteria)
    {
        _repository = repository;
        _criteria = criteria;
    }

    public async Task<Guid> Handle(
        UpdateLocationAddressCommand command,
        CancellationToken ct = default)
    {
        var locationId = LocationId.Create(command.Id);
        
        Domain.LocationContext.Location? location = await _repository.GetByIdAsync(locationId, ct);
        if (location is null)
        {
            string message = $"Локация с идентификатором {command.Id} не найдена";
            throw new InvalidOperationException(message);
        }

        LocationAddress address = LocationAddress.Create(command.Address);
        
        // Проверяем уникальность нового адреса
        location.ChangeLocationAddress(_criteria, address);
        
        await _repository.UpdateAsync(location, ct);
        
        return location.Id.Value;
    }
}
