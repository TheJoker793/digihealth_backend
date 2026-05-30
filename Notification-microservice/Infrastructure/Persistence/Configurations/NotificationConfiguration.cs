using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_microservice.Domain.Entities;

namespace Notification_microservice.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Numero)
                .IsRequired().HasMaxLength(30);
            builder.HasIndex(n => n.Numero).IsUnique();

            builder.Property(n => n.TypeEvenement)
                .HasConversion<string>().HasMaxLength(40);

            builder.Property(n => n.Canal)
                .HasConversion<string>().HasMaxLength(10);

            builder.Property(n => n.Statut)
                .HasConversion<string>().HasMaxLength(15);

            builder.Property(n => n.TypeDestinataire)
                .IsRequired().HasMaxLength(20);

            builder.Property(n => n.Sujet)
                .IsRequired().HasMaxLength(200);

            builder.Property(n => n.CorpsRendu)
                .IsRequired().HasColumnType("nvarchar(max)");

            builder.Property(n => n.ContactEmail).HasMaxLength(150);
            builder.Property(n => n.ContactTelephone).HasMaxLength(20);
            builder.Property(n => n.TokenFcm).HasMaxLength(200);
            builder.Property(n => n.PieceJointeChemin).HasMaxLength(500);
            builder.Property(n => n.DerniereErreur).HasMaxLength(1000);

            // Index pour les requêtes fréquentes
            builder.HasIndex(n => new { n.DestinataireId, n.Statut });
            builder.HasIndex(n => new { n.Statut, n.DateProgrammee });
            builder.HasIndex(n => n.SourceId);

            // Composition HistoriqueEnvoi
            builder.HasMany(n => n.Historiques)
                .WithOne(h => h.Notification)
                .HasForeignKey(h => h.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(n => n.DomainEvents);
        }
    }
}
