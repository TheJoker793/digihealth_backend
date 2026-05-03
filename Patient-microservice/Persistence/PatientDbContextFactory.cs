using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Patient_microservice.Persistence;

public class PatientDbContextFactory : IDesignTimeDbContextFactory<PatientDbContext>
{
    public PatientDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PatientDbContext>();
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-700SABG\\SQLEXPRESS;Initial Catalog=digihealth_patient;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");
        return new PatientDbContext(optionsBuilder.Options);
    }
}
