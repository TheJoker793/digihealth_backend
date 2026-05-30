using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Infrastructure.Persistence.Configurations
{
    public class TemplateNotificationConfiguration : IEntityTypeConfiguration<TemplateNotification>
    {
        public void Configure(EntityTypeBuilder<TemplateNotification> builder)
        {
            builder.ToTable("TemplatesNotification");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code)
                .IsRequired().HasMaxLength(100);
            builder.HasIndex(t => t.Code).IsUnique();

            builder.Property(t => t.TypeEvenement)
                .HasConversion<string>().HasMaxLength(40);

            builder.Property(t => t.Canal)
                .HasConversion<string>().HasMaxLength(10);

            builder.Property(t => t.Langue)
                .IsRequired().HasMaxLength(5);

            builder.Property(t => t.SujetTemplate)
                .IsRequired().HasMaxLength(200);

            builder.Property(t => t.CorpsTemplate)
                .IsRequired().HasColumnType("nvarchar(max)");

            // string[] Variables → stocké en CSV
            builder.Property(t => t.Variables)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .HasMaxLength(2000);

            builder.HasIndex(t => new { t.TypeEvenement, t.Canal, t.Langue }).IsUnique();
            builder.HasIndex(t => t.EstActif);

            builder.Ignore(t => t.DomainEvents);
        }
    }
}
