using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Infrastructure.Persistence.Configurations
{
    public class HistoriqueEnvoiConfiguration : IEntityTypeConfiguration<HistoriqueEnvoi>
    {
        public void Configure(EntityTypeBuilder<HistoriqueEnvoi> builder)
        {
            builder.ToTable("HistoriquesEnvoi");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Canal)
                .HasConversion<string>().HasMaxLength(10);

            builder.Property(h => h.Resultat)
                .HasConversion<string>().HasMaxLength(20);

            builder.Property(h => h.MessageErreur).HasMaxLength(1000);
            builder.Property(h => h.ProviderResponse).HasMaxLength(200);

            builder.HasIndex(h => h.NotificationId);
            builder.HasIndex(h => h.DateTentative);

            builder.Ignore(h => h.DomainEvents);
        }
    }
}

