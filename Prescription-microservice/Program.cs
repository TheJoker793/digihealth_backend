using Prescription_microservice.Extensions;
using Prescription_microservice.Infrastructure.Middlewares;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Services
// =========================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddDatabase(connectionString)
    .AddRepositories()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddMessaging(builder.Configuration)
    .AddJwtFromAuthSvc(builder.Configuration)
    .AddHangfireJobs(connectionString)
    .AddValidators();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =========================
// Pipeline HTTP
// =========================
app.UseExceptionHandling();   // ton middleware custom
app.UseCorrelationId();       // ajoute X-Correlation-Id

app.UseHttpsRedirection();

app.UseAuthentication();      // JWT
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Prescription API v1");
    options.RoutePrefix = "swagger";
});

// =========================
// Endpoints
// =========================
app.MapControllers();

// Hangfire Dashboard (protégé par JWT/roles)
// Dashboard Hangfire sans filtre custom
app.MapHangfireDashboard("/hangfire");


app.Run();
