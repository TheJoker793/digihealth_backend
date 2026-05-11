using DMI.RendezVous.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Rendez_vous_microservice.Domain.Interfaces;
using Rendez_vous_microservice.Exceptions;
using Rendez_vous_microservice.Extensions;
using Rendez_vous_microservice.Infrastructure.Persistence;
using Rendez_vous_microservice.Infrastructure.Persistence.Services;
using MassTransit;
using StackExchange.Redis;
using static Rendez_vous_microservice.Infrastructure.Persistence.Services.SignalRRdvNotifier;

var builder = WebApplication.CreateBuilder(args);

#region Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region DB
builder.Services.AddDbContext<RendezVousDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region MassTransit / RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});
// ← AddMassTransitHostedService() supprimé (obsolète, enregistré automatiquement)
#endregion

#region Redis
var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse(redisConnection);
    config.AbortOnConnectFail = false; // ne plante pas au démarrage si Redis absent
    return ConnectionMultiplexer.Connect(config);
});
#endregion

#region SignalR
builder.Services.AddSignalR();
#endregion

#region Custom Services
builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<IRdvCacheService, RedisRdvCacheService>();
builder.Services.AddScoped<IRdvNotifier, SignalRRdvNotifier>();
#endregion

#region Infrastructure
builder.Services.AddInfrastructure();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
// UseHttpsRedirection supprimé — inutile en Docker HTTP interne
app.UseAuthorization();
app.MapControllers();
app.MapHub<RendezVousHub>("/rendezvousHub");

app.Run();