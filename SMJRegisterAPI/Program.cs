using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Features.BanksInformation.Repository;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.Church.Repository;
using SMJRegisterAPI.Features.Common;
using SMJRegisterAPI.Features.GrantedCode.Repository;
using SMJRegisterAPI.Features.Room.Repository;
using SMJRegisterAPI.Middlewares;
using SMJRegisterAPI.Services.CodeGenerator;
using SMJRegisterAPI.Services.FileStore;

var builder = WebApplication.CreateBuilder(args);

#region DbContext Configurations

builder.Services.AddDbContext<ApplicationDbContext>(opt=>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);


#endregion

#region Repositories and services 
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped(typeof(ICamperRepository),typeof(CamperRepository));
    builder.Services.AddScoped(typeof(IChurchRepository),typeof(ChurchRepository));
    builder.Services.AddScoped(typeof(IRoomRepository),typeof(RoomRepository));
    builder.Services.AddScoped(typeof(IGenerateCodeService),typeof(GenerateCodeService));
    builder.Services.AddScoped(typeof(IGrantedCodeRepository),typeof(GrantedCodeRepository));
    builder.Services.AddScoped(typeof(IBankInformationRepository),typeof(BankInformationRepository));
    builder.Services.AddScoped(typeof(IFileStorage),typeof(FileStorage));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddHttpContextAccessor();

#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
#endregion

#region Automapper y MediatR

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, ServiceLifetime.Scoped);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddMediatR(cfg => 
        cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
#endregion
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCarter(configurator: config =>
{
    config.WithValidatorLifetime(ServiceLifetime.Scoped);
});
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
app.UseCors();
#region Error middleware

app.UseExceptionHandler(cfg =>
{
    cfg.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is ValidationException ve)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ve.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new
            {
                errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        else
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = exception?.Message });
        }
    });
});
#endregion


app.Run();
