using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using Domain.InMemory;
using Asp.NET;
using Microsoft.AspNetCore.Mvc;
using Application.Position;
using Application;

namespace Asp.NET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly RegisterPositionHandler _handler;

    public PositionsController(RegisterPositionHandler handler)
    {
        _handler = handler;
    }

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
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreatePositionRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var command = new RegisterPositionCommand(request.Name);
            var positionId = await _handler.Handle(command, ct);
            
            return CreatedAtAction(nameof(GetById), new { id = positionId }, positionId);
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

            InMemoryPositionRepository.Remove(positionId);
            
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
