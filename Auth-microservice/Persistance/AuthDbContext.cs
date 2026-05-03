using Auth_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth_microservice.Persistance
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
        {
        }

        // =========================
        // DBSets
        // =========================
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        // =========================
        // MODEL CONFIGURATION
        // =========================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        }

        // =========================
        // SAVE CHANGES (AUDIT AUTOMATIQUE)
        // =========================
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditing();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        // =========================
        // AUDIT (CreatedAt / UpdatedAt)
        // =========================
        private void ApplyAuditing()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
        }

    }
}
