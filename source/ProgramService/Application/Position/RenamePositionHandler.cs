using Domain.PositionsContext;
using Domain.PositionsContext.Repositories;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;

namespace Application.Position;

public sealed class RenamePositionHandler
{
    private readonly IPositionRepository _repository;
    private readonly IPositionNameUniquenessCriteria _criteria;

    public RenamePositionHandler(
        IPositionRepository repository,
        IPositionNameUniquenessCriteria criteria)
    {
        _repository = repository;
        _criteria = criteria;
    }

    public async Task<Guid> Handle(
        RenamePositionCommand command,
        CancellationToken ct = default)
    {
        var positionId = PositionId.Create(command.Id);
        
        Domain.PositionsContext.Position? position = await _repository.GetByIdAsync(positionId, ct);
        if (position is null)
        {
            string message = $"Должность с идентификатором {command.Id} не найдена";
            throw new InvalidOperationException(message);
        }

        NotEmptyName name = NotEmptyName.Create(command.NewName);
        
        // Проверяем уникальность нового имени
        position.ChangePositionName(_criteria, name);
        
        await _repository.UpdateAsync(position, ct);
        
        return position.Id.Value;
    }
}
