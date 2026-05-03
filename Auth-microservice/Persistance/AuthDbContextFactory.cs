using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Auth_microservice.Persistance
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();

            optionsBuilder.UseSqlServer(
                "Data Source=DESKTOP-700SABG\\SQLEXPRESS;Initial Catalog=digihealth;Integrated Security=True;TrustServerCertificate=True");

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
}
