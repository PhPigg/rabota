using Microsoft.AspNetCore.Mvc;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using Domain.InMemory;
using static Domain.PositionsContext.ValueObjects.PositionDescription;

namespace Asp.NET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    // GET: api/positions
    [HttpGet]
    public ActionResult<IEnumerable<Position>> GetAll()
    {
        var positions = InMemoryPositionRepository.GetAll();
        return Ok(positions);
    }

    // GET: api/positions/{id}
    [HttpGet("{id}")]
    public ActionResult<Position> GetById(Guid id)
    {
        try
        {
            var positionId = PositionId.Create(id);
            var position = InMemoryPositionRepository.GetById(positionId);
            
            if (position == null)
            {
                return NotFound();
            }

            if (!position.LifeTime.IsActive)
            {
                return NotFound();
            }

            return Ok(position);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/positions
    [HttpPost]
    public ActionResult<Position> Create([FromBody] CreatePositionRequest request)
    {
        try
        {
            // Валидация входных данных
            if (request == null)
            {
                return BadRequest("Запрос не может быть пустым.");
            }

            var name = NotEmptyName.Create(request.Name);
            var description = PositionDescription.Create(request.Description);
            var criteria = new PositionNameUniquenessCriteria();
            
            var position = Position.CreateNew(criteria, name, description);
            InMemoryPositionRepository.Add(position, criteria);
            
            return CreatedAtAction(nameof(GetById), new { id = position.Id.Value }, position);
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

    // PUT: api/positions/{id}
    [HttpPut("{id}")]
    public ActionResult<Position> Update(Guid id, [FromBody] UpdatePositionRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest("Запрос не может быть пустым.");
            }

            var positionId = PositionId.Create(id);
            var position = InMemoryPositionRepository.GetById(positionId);
            
            if (position == null)
            {
                return NotFound();
            }

            if (!position.LifeTime.IsActive)
            {
                return NotFound();
            }

            var criteria = new PositionNameUniquenessCriteria();
            var newName = NotEmptyName.Create(request.Name);
            
            position.ChangePositionName(criteria, newName);
            
            return Ok(position);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("архиве"))
            {
                return NotFound();
            }
            return Conflict(ex.Message);
        }
    }

    // DELETE: api/positions/{id} (soft delete / archive)
    [HttpDelete("{id}")]
    public ActionResult<Position> Delete(Guid id)
    {
        try
        {
            var positionId = PositionId.Create(id);
            var position = InMemoryPositionRepository.GetById(positionId);
            
            if (position == null)
            {
                return NotFound();
            }

            if (!position.LifeTime.IsActive)
            {
                return NotFound();
            }

            // Архивация сущности
            position.Archive();
            
            return Ok(position);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/positions/{id}/hard (hard delete)
    [HttpDelete("{id}/hard")]
    public ActionResult HardDelete(Guid id)
    {
        try
        {
            var positionId = PositionId.Create(id);
            var position = InMemoryPositionRepository.GetById(positionId);
            
            if (position == null)
            {
                return NotFound();
            }

            InMemoryPositionRepository.HardRemove(positionId);
            
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreatePositionRequest(string Name, string Description);
public record UpdatePositionRequest(string Name);
