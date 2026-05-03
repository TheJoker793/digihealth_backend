using DMI.RendezVous.Infrastructure.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Domain.Interfaces;
using Rendez_vous_microservice.Exceptions;
using Rendez_vous_microservice.Infrastructure.Hubs;
using Rendez_vous_microservice.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// ✅ DbContext EF Core + SQL Server
builder.Services.AddDbContext<RendezVousDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddApplicationServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandling();
app.UseJwtValidation();


app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapHub<AgendaHub>("/agendaHub");   // SignalR Hub
app.MapHangfireDashboard();            // Dashboard Hangfire


app.Run();
