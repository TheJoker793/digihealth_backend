using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_microservice.Domain.Entities;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Infrastructure.Persistence.Configurations
{
    public class PreferenceNotificationConfiguration : IEntityTypeConfiguration<PreferenceNotification>
    {
        public void Configure(EntityTypeBuilder<PreferenceNotification> builder)
        {
            builder.ToTable("PreferencesNotification");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.TypeDestinataire)
                .IsRequired().HasMaxLength(20);

            builder.Property(p => p.Langue)
                .IsRequired().HasMaxLength(5);

            // CanalEnvoi[] → stocké en CSV  ex: "Email,SMS"
            builder.Property(p => p.CanauxActifs)
                .HasConversion(
                    v => string.Join(',', v.Select(c => c.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(Enum.Parse<CanalEnvoi>).ToArray())
                .HasMaxLength(100);

            builder.Property(p => p.CanauxOptOut)
                .HasConversion(
                    v => string.Join(',', v.Select(c => c.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(Enum.Parse<CanalEnvoi>).ToArray())
                .HasMaxLength(100);

            builder.HasIndex(p => new { p.DestinataireId, p.TypeDestinataire }).IsUnique();

            builder.Ignore(p => p.DomainEvents);
        }
    }
}
