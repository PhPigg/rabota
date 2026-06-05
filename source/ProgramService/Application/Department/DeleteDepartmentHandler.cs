using Domain.DepartmentContext.Repositories;
using Domain.DepartmentContext.ValueObject;

namespace Application.Department;

/**
 * <summary>
 * Обработчик команды удаления подразделения.
 * </summary>
 */
public sealed class DeleteDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public DeleteDepartmentHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    /**
     * <summary>
     * Выполняет команду удаления подразделения.
     * </summary>
     * <param name="command">Команда с идентификатором подразделения.</param>
     * <param name="ct">Токен отмены операции.</param>
     * <returns>Задача, представляющая асинхронную операцию.</returns>
     */
    public async Task Handle(DeleteDepartmentCommand command, CancellationToken ct = default)
    {
        var departmentId = DepartmentId.Create(command.Id);
        await _repository.DeleteAsync(departmentId, ct);
    }
}

/**
 * <summary>
 * Команда удаления подразделения.
 * </summary>
 * <param name="Id">Идентификатор подразделения для удаления.</param>
 */
public record DeleteDepartmentCommand(Guid Id);
