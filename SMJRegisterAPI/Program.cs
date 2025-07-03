using Carter;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SMJRegisterAPI.Database.Contexts;

var builder = WebApplication.CreateBuilder(args);

#region DbContext Configurations

builder.Services.AddDbContext<ApplicationDbContext>(opt=>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);


#endregion

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCarter();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options=> 
        options
            .WithTitle("SmjRegisterAPI")
            .WithTheme(ScalarTheme.DeepSpace)
    );
}

app.UseHttpsRedirection();
app.MapCarter();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}