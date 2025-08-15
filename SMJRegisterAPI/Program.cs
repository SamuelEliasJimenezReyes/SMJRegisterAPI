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

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT")?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");
builder.Services.AddHealthChecks();
#region DbContext Configurations
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion

#region Repositories and Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICamperRepository, CamperRepository>();
builder.Services.AddScoped<IChurchRepository, ChurchRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IGenerateCodeService, GenerateCodeService>();
builder.Services.AddScoped<IGrantedCodeRepository, GrantedCodeRepository>();
builder.Services.AddScoped<IBankInformationRepository, BankInformationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddScoped<ITenantServices, TenantServices>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenServices>();
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



#region Swagger y Carter
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter();

#endregion


#region Auth
var jwtKey = builder.Configuration["JwtSettings:Key"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];

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
app.UseHealthChecks("/health");

#region Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

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

            await context.Response.WriteAsJsonAsync(new { errors });
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
