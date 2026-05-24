using Dossier_Medical_microservice.Extensions;
using Dossier_Medical_microservice.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Connection string
var connectionString = builder.Configuration.GetConnectionString("DossierDatabase");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("La chaîne de connexion 'DossierDatabase' est introuvable.");

// ✅ Enregistrement des couches
builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtFromAuthSvc(builder.Configuration);

// ✅ Controllers / API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dossier Médical API",
        Version = "v1",
        Description = "API de gestion des dossiers médicaux",
        Contact = new OpenApiContact
        {
            Name = "Equipe DigiHealth",
            Email = "support@digihealth.tn"
        }
    });

    // 🔹 Ajout du bouton Authorize pour JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

    // ✅ Ajout du support upload fichier
});

// ✅ SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// ✅ Middlewares
app.UseExceptionHandling();
app.UseJwtValidation();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dossier Médical API v1");
    options.RoutePrefix = "swagger"; // accessible via /swagger
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// ✅ Endpoints
app.MapControllers();

app.Run();
