using Document_microservice.Extensions;
using Document_microservice.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing DefaultConnection");
builder.Services
.AddDatabase(connectionString)
.AddUnitOfWork()
.AddApplicationServices()
.AddInfrastructureServices(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();   // ton middleware custom
app.UseCorrelationId();       // ajoute X-Correlation-Id
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
