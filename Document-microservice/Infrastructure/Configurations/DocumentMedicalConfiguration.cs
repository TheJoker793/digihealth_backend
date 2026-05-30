using Document_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Document_microservice.Infrastructure.Configurations
{
    public class DocumentMedicalConfiguration : IEntityTypeConfiguration<DocumentMedical>
    {
        public void Configure(EntityTypeBuilder<DocumentMedical> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Numero)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(d => d.Numero).IsUnique();

            builder.Property(d => d.Titre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .HasMaxLength(1000);

            builder.Property(d => d.TypeDocument)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(d => d.Statut)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(d => d.PatientId);
            builder.HasIndex(d => d.MedecinId);
            builder.HasIndex(d => d.CabinetId);
            builder.HasIndex(d => new { d.CabinetId, d.TypeDocument, d.Statut });

            // Composition — versions
            builder.HasMany(d => d.Versions)
                .WithOne(v => v.DocumentMedical)
                .HasForeignKey(v => v.DocumentMedicalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Composition — partages
            builder.HasMany(d => d.Partages)
                .WithOne(p => p.DocumentMedical)
                .HasForeignKey(p => p.DocumentMedicalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignorer les domain events (non persistés)
            builder.Ignore(d => d.DomainEvents);
        }
    }
}
