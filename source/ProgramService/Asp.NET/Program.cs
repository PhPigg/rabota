using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.InMemory;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "API Documentation",
        Version = "v1",
        Description = "API для управления локациями и должностями"
    });
});

// Add controllers
builder.Services.AddControllers();

// Configure routing options for Swagger compatibility
builder.Services.Configure<RouteOptions>(options =>
{
    options.SetParameterPolicy<Microsoft.AspNetCore.Routing.Constraints.RegexInlineRouteConstraint>("regex");
});

var app = builder.Build();

// Configure routing and controllers
app.UseRouting();
app.MapControllers();

// Configure Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

// Initialize storage with seed data
InitializeStorage();

app.Run();

// Storage initialization method
static void InitializeStorage()
{
    var locationCriteria = new LocationUniquenessCriteria();
    var positionCriteria = new PositionNameUniquenessCriteria();
    
    InMemoryLocationRepository.InitializeSeedData(locationCriteria);
    InMemoryPositionRepository.InitializeSeedData(positionCriteria);
}

// Uniqueness criteria implementations
public class LocationUniquenessCriteria : ILocationUniquenessCriteria
{
    public bool IsSatisfiedBy(NotEmptyName name)
    {
        var existing = InMemoryLocationRepository.GetAll();
        return !existing.Any(l => l.Id != LocationId.CreateNew() && l.Name.Value == name.Value);
    }

    public bool IsSatisfiedBy(LocationAddress address)
    {
        var existing = InMemoryLocationRepository.GetAll();
        return !existing.Any(l => l.Id != LocationId.CreateNew() && l.Address.Value == address.Value);
    }
}

public class PositionNameUniquenessCriteria : IPositionNameUniquenessCriteria
{
    public bool IsSatisfiedBy(NotEmptyName name)
    {
        var existing = InMemoryPositionRepository.GetAll();
        return !existing.Any(p => p.Id != PositionId.CreateNew() && p.Name.Value == name.Value);
    }
}

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);
