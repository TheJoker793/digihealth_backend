using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Document_microservice.Infrastructure.Persistence;

/// <summary>
/// Utilisé uniquement par les commandes EF Core (Add-Migration, Update-Database).
/// Lit la connection string depuis appsettings.json.
/// </summary>
public class DocumentDbContextFactory : IDesignTimeDbContextFactory<DocumentDbContext>
{
    public DocumentDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DocumentDbContext>();
        optionsBuilder.UseSqlServer(
            config.GetConnectionString("DefaultConnection"),
            sql => sql.MigrationsAssembly(typeof(DocumentDbContext).Assembly.FullName));

        return new DocumentDbContext(optionsBuilder.Options);
    }
}