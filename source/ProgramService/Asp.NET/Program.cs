using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.InMemory;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;
using Asp.NET;
using Infostructure;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Options;




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


builder.Services.AddOptions<db_connectionsoptions>().BindConfiguration("db_connectionsoptions");

// Add controllers
builder.Services.AddControllers();

// Configure routing options for Swagger compatibility
builder.Services.Configure<RouteOptions>(options =>
{
    options.SetParameterPolicy<Microsoft.AspNetCore.Routing.Constraints.RegexInlineRouteConstraint>("regex");
});

var app = builder.Build();
using var scope = app.Services.CreateScope();
var options = scope.ServiceProvider.GetRequiredService<IOptions<db_connectionsoptions>>();
Console.WriteLine($"{options.Value.host}\n{options.Value.username}\n{options.Value.password}\n{options.Value.database}\n{options.Value.port}");


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
