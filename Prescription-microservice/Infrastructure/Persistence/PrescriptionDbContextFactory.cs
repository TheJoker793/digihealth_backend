using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Prescription_microservice.Infrastructure.Persistence
{
    public class PrescriptionDbContextFactory : IDesignTimeDbContextFactory<PrescriptionDbContext>
    {
        public PrescriptionDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PrescriptionDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new PrescriptionDbContext(optionsBuilder.Options);
        }
    }
}
