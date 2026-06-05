using Domain.PositionsContext.Repositories;
using Domain.PositionsContext.ValueObjects;

namespace Application.Position;

/**
 * <summary>
 * Обработчик команды удаления должностей по массиву идентификаторов.
 * </summary>
 */
public sealed class DeletePositionsHandler
{
    private readonly IPositionRepository _repository;

    public DeletePositionsHandler(IPositionRepository repository)
    {
        _repository = repository;
    }

    /**
     * <summary>
     * Выполняет команду удаления должностей.
     * </summary>
     * <param name="command">Команда с массивом идентификаторов должностей.</param>
     * <param name="ct">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    public async Task Handle(DeletePositionsCommand command, CancellationToken ct = default)
    {
        var ids = command.Ids.Select(id => PositionId.Create(id)).ToList();
        await _repository.DeleteManyAsync(ids, ct);
    }
}

/**
 * <summary>
 * Команда удаления должностей.
 * </summary>
 * <param name="Ids">Массив идентификаторов должностей для удаления.</param>
 */
public record DeletePositionsCommand(Guid[] Ids);
