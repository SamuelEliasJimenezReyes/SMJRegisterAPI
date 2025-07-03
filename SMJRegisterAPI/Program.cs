using System.Reflection;
using Carter;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.Common;

var builder = WebApplication.CreateBuilder(args);

#region DbContext Configurations

builder.Services.AddDbContext<ApplicationDbContext>(opt=>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);


#endregion

#region Repositories
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped(typeof(ICamperRepository),typeof(CamperRepository));
#endregion

#region Automapper y MediatR

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddMediatR(cfg => 
        cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
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

app.Run();
