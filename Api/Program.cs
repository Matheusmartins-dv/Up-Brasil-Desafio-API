using Application.Features.V1.Users;
using Carter;
using Infra.Data.Context;
using Infra.Data;
using Domain.Interfaces;
using Application.Common.Services;
using Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCarter();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("DefaultConnection not found.");

builder.Services.AddDataBase(connectionString);

builder.Services.AddScoped<IUserValidationService, UserValidationService>();
builder.Services.AddScoped<IProductValidationService, ProductValidationService>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<UpContext>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapCarter();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
