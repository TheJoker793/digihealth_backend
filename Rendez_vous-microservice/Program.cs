using Rendez_vous_microservice.Extensions;   // ✅ Importer tes extensions
using Rendez_vous_microservice.Infrastructure.Hubs;
using Rendez_vous_microservice.Exceptions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// ✅ Récupération de la chaîne de connexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' est introuvable dans appsettings.json.");
}

// ✅ Enregistrement des couches via extensions
builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddHangfire(connectionString);
builder.Services.AddSignalR();

// ✅ Controllers / API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();   // middleware custom
app.UseJwtValidation();       // middleware JWT

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// ✅ Endpoints
app.MapControllers();
app.MapHub<AgendaHub>("/agendaHub");   // SignalR Hub
app.MapHangfireDashboard();            // Dashboard Hangfire

app.Run();
