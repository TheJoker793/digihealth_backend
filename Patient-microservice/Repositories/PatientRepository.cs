using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces;
using Patient_microservice.Persistence;

namespace Patient_microservice.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(PatientDbContext context) : base(context)
        {
        }
    }
}
