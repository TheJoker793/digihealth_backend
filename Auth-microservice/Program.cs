using Auth_microservice.Configurations;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Persistance;
using Auth_microservice.Repositories;
using Auth_microservice.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Auth_microservice.Exceptions;
using Auth_microservice.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region =========================
// CONTROLLERS + SWAGGER
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region =========================
// DB CONTEXT
#endregion

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region =========================
// REPOSITORIES
#endregion

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

#region =========================
// UNIT OF WORK
#endregion

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#region =========================
// APPLICATION SERVICES
#endregion

builder.Services.AddScoped<AuthService>();

#region =========================
// INFRA SERVICES
#endregion

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<ITotpService, OtpNetTotpService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

#region =========================
// REDIS (SAFE - LAZY)
#endregion


#region =========================
// RABBITMQ (MASSTRANSIT)
#endregion

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });
    });
});

#region =========================
// JWT AUTH
#endregion

var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

#region =========================
// APP
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
