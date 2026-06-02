using Domain.PositionsContext;
using Domain.PositionsContext.Repositories;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using DomainPosition = Domain.PositionsContext.Position;

namespace Application.Position;

public sealed class RegisterPositionHandler
{
    private readonly IPositionRepository _repository;
    private readonly IPositionNameUniquenessCriteria _criteria;

    public RegisterPositionHandler(
        IPositionRepository repository,
        IPositionNameUniquenessCriteria criteria)
    {
        _repository = repository;
        _criteria = criteria;
    }

    public async Task<Guid> Handle(
        RegisterPositionCommand command,
        CancellationToken ct = default)
    {
        ValidatePositionName(command);
        var position = CreatePosition(command);
        await _repository.AddAsync(position, ct);
        return position.Id.Value;
    }

    private void ValidatePositionName(RegisterPositionCommand command)
    {
        string name = command.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Название должности не может быть пустым.");
        }
        const int maxLength = 255;
        if (name.Length > maxLength)
        {
            throw new InvalidOperationException($"Название должности слишком длинное ({name.Length} символов). Максимально допустимое количество символов: {maxLength}.");
        }
    }

    private DomainPosition CreatePosition(RegisterPositionCommand command)
    {
        // Создаем value objects из строковых данных команды
        var name = NotEmptyName.Create(command.Name);
        
        // Используем статический фабричный метод сущности
        var position = DomainPosition.CreateNew(_criteria, name);
        
        return position;
    }
}
