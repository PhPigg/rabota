using Application;
using Application.Department;
using Asp.NET;
using Domain.DepartmentContext;
using Domain.DepartmentContext.Repositories;
using Domain.DepartmentContext.ValueObject;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repository;
    private readonly DeleteDepartmentHandler _handler;

    public DepartmentsController(
        IDepartmentRepository repository,
        DeleteDepartmentHandler handler)
    {
        _repository = repository;
        _handler = handler;
    }

    // GET: api/departments
    [HttpGet]
    public ActionResult<IEnumerable<Department>> GetAll()
    {
        return Ok(_repository.GetAllAsync().Result);
    }

    // GET: api/departments/{id}
    [HttpGet("{id}")]
    public ActionResult<Department> GetById(Guid id)
    {
        try
        {
            var departmentId = DepartmentId.Create(id);
            var department = _repository.GetByIdAsync(departmentId).Result;
            
            if (department == null)
            {
                return NotFound();
            }

            if (!department.LifeTime.IsActive)
            {
                return NotFound();
            }

            return Ok(department);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/departments
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken ct = default)
    {
        try
        {
            // TODO: Implement department creation logic
            // For now, return a placeholder
            return BadRequest("Создание подразделений пока не реализовано.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    // DELETE: api/departments/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        try
        {
            var command = new DeleteDepartmentCommand(id);
            await _handler.Handle(command, ct);
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreateDepartmentRequest(string Name, Guid? ParentId = null);
