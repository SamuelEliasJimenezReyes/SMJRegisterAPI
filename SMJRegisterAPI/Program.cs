using System.Security.Claims;
using System.Text;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Entities;
using SMJRegisterAPI.Features.BanksInformation.Repository;
using SMJRegisterAPI.Features.Camper.Repository;
using SMJRegisterAPI.Features.Church.Repository;
using SMJRegisterAPI.Features.Common;
using SMJRegisterAPI.Features.GrantedCode.Repository;
using SMJRegisterAPI.Features.Payment.Repository;
using SMJRegisterAPI.Features.Room.Repository;
using SMJRegisterAPI.Middlewares;
using SMJRegisterAPI.Services.CodeGenerator;
using SMJRegisterAPI.Services.FileStore;
using SMJRegisterAPI.Services.Tenant;
using SMJRegisterAPI.Services.User;
using SMJRegisterAPI.Swagger;

var builder = WebApplication.CreateBuilder(args);

#region DbContext Configurations

builder.Services.AddDbContext<ApplicationDbContext>(opt=>
    opt.UseNpgsql(
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
    builder.Services.AddScoped(typeof(IPaymentRepository),typeof(PaymentRepository));
    builder.Services.AddScoped(typeof(IFileStorage),typeof(FileStorage));
    builder.Services.AddScoped(typeof(ITenantServices),typeof(TenantServices));
    builder.Services.AddScoped(typeof(IJwtTokenService),typeof(JwtTokenServices));
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

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddMediatR(cfg => 
        cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
#endregion

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddCarter(configurator: config =>
{
    config.WithValidatorLifetime(ServiceLifetime.Scoped);
});


#region Auth
// JWT
var jwtKey = builder.Configuration["JwtSettings:Key"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtDuration = builder.Configuration["JwtSettings:DurationInMinutes"];

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            RoleClaimType = "conference",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.MapOpenApi();
    app.MapScalarApiReference(options=> 
        options
            .WithTitle("SmjRegisterAPI")
            .WithTheme(ScalarTheme.DeepSpace)
    );

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
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