using Application;
using Application.Location;
using Asp.NET;
using Domain.InMemory;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NET.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly RegisterLocationHandler _handler;

    public LocationsController(RegisterLocationHandler handler)
    {
        _handler = handler;
    }

    // GET: api/locations
    [HttpGet]
    public ActionResult<IEnumerable<Location>> GetAll()
    {
        var locations = InMemoryLocationRepository.GetAll();
        return Ok(locations);
    }

    // GET: api/locations/{id}
    [HttpGet("{id}")]
    public ActionResult<Location> GetById(Guid id)
    {
        try
        {
            var locationId = LocationId.Create(id);
            var location = InMemoryLocationRepository.GetById(locationId);
            
            if (location == null)
            {
                return NotFound();
            }

            if (!location.LifeTime.IsActive)
            {
                return NotFound();
            }

            return Ok(location);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/locations
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateLocationRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var command = new RegisterLocationCommand(
                new[] { request.Name },
                request.Address,
                request.TimeZone
            );
            var locationId = await _handler.Handle(command, ct);
            
            return CreatedAtAction(nameof(GetById), new { id = locationId }, locationId);
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

    // PUT: api/locations/{id}
    [HttpPut("{id}")]
    public ActionResult<Location> Update(Guid id, [FromBody] UpdateLocationRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest("Запрос не может быть пустым.");
            }

            var locationId = LocationId.Create(id);
            var location = InMemoryLocationRepository.GetById(locationId);
            
            if (location == null)
            {
                return NotFound();
            }

            if (!location.LifeTime.IsActive)
            {
                return NotFound();
            }

            var criteria = new LocationUniquenessCriteria();
            
            if (!string.IsNullOrEmpty(request.Name))
            {
                var newName = NotEmptyName.Create(request.Name);
                location.ChangeLocationName(criteria, newName);
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                var newAddress = LocationAddress.Create(request.Address);
                location.ChangeLocationAddress(criteria, newAddress);
            }

            if (!string.IsNullOrEmpty(request.TimeZone))
            {
                var newTimeZone = IanaTimeZone.Create(request.TimeZone);
                location.ChangeIanaTimeZone(newTimeZone);
            }
            
            return Ok(location);
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

    // DELETE: api/locations/{id} (soft delete / archive)
    [HttpDelete("{id}")]
    public ActionResult<Location> Delete(Guid id)
    {
        try
        {
            var locationId = LocationId.Create(id);
            var location = InMemoryLocationRepository.GetById(locationId);
            
            if (location == null)
            {
                return NotFound();
            }

            if (!location.LifeTime.IsActive)
            {
                return NotFound();
            }

            // Архивация сущности
            location.Archive();
            
            return Ok(location);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/locations/{id}/hard (hard delete)
    [HttpDelete("{id}/hard")]
    public ActionResult HardDelete(Guid id)
    {
        try
        {
            var locationId = LocationId.Create(id);
            var location = InMemoryLocationRepository.GetById(locationId);
            
            if (location == null)
            {
                return NotFound();
            }

            InMemoryLocationRepository.Remove(locationId);
            
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreateLocationRequest(string Name, string Address, string TimeZone);
public record UpdateLocationRequest(string? Name, string? Address, string? TimeZone);

public record LocationResponse(string Name, string Address, string TimeZone, DateTime CreatedAt, DateTime? UpdatedAt, DateTime? DeletedAt, bool IsActive);
