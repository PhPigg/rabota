using Domain.LocationContext.Repositories;
using Domain.LocationContext.ValueObjects;

namespace Application.Location;

/**
 * <summary>
 * Обработчик команды удаления локации.
 * </summary>
 */
public sealed class DeleteLocationHandler
{
    private readonly ILocationRepository _repository;

    public DeleteLocationHandler(ILocationRepository repository)
    {
        _repository = repository;
    }

    /**
     * <summary>
     * Выполняет команду удаления локации.
     * </summary>
     * <param name="command">Команда с идентификатором локации.</param>
     * <param name="ct">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    public async Task Handle(DeleteLocationCommand command, CancellationToken ct = default)
    {
        var locationId = LocationId.Create(command.Id);
        await _repository.DeleteAsync(locationId, ct);
    }
}

/**
 * <summary>
 * Команда удаления локации.
 * </summary>
 * <param name="Id">Идентификатор локации для удаления.</param>
 */
public record DeleteLocationCommand(Guid Id);
