using System.Text.Json.Serialization;
using Domain.InMemory;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using Asp.NET;
using Infostructure;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Asp.NET.Extension;






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
