using System.Text.Json.Serialization;
using Domain.InMemory;
using Domain.LocationContext.Repositories;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.PositionsContext.Repositories;
using Domain.DepartmentContext.Repositories;
using Domain.Shared;
using Asp.NET;
using Infostructure;
using Infostructure.Database;
using Asp.NET.Extension;
using Application;






var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOptions<db_connectionsoptions>().BindConfiguration("db_connectionsoptions");

// Add controllers
builder.Services.AddControllers();

// Register repositories and handlers
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// Register Application handlers
builder.Services.AddScoped<Application.Position.RegisterPositionHandler>();
builder.Services.AddScoped<Application.Location.RegisterLocationHandler>();
builder.Services.AddScoped<Application.Department.DeleteDepartmentHandler>();
builder.Services.AddScoped<Application.Position.DeletePositionsHandler>();
builder.Services.AddScoped<Application.Location.DeleteLocationHandler>();





WebApplication app = builder.Build();
await app.BuildDatabase();

// Configure Swagger middleware (must be before UseRouting and MapControllers)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger";
    });
}

// Configure routing and controllers
app.UseRouting();
app.MapControllers();

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

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);
