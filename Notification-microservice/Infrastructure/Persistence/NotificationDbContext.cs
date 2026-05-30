using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Infrastructure.Persistence
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options) { }

        // ── DbSets ───────────────────────────────────────────────
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<HistoriqueEnvoi> Historiques => Set<HistoriqueEnvoi>();
        public DbSet<TemplateNotification> Templates => Set<TemplateNotification>();
        public DbSet<PreferenceNotification> Preferences => Set<PreferenceNotification>();

        // ── Configuration ────────────────────────────────────────
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        }

        // ── Audit automatique UpdatedAt ──────────────────────────
        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                if (entry.State == EntityState.Modified)
                    entry.Property("UpdatedAt").CurrentValue = DateTimeOffset.UtcNow;

            return await base.SaveChangesAsync(ct);
        }



    }
}
