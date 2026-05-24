using Microsoft.EntityFrameworkCore;
using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Infrastructure.Persistence
{
    public class PrescriptionDbContext : DbContext
    {
        public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options)
            : base(options) { }

        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<LignePrescription> Lignes => Set<LignePrescription>();
        public DbSet<InteractionDetectee> Interactions => Set<InteractionDetectee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Audit automatique
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTimeOffset>("CreatedAt")
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTimeOffset?>("UpdatedAt");
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
