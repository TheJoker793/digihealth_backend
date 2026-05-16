using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dossier_Medical_microservice.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentMedical>
    {
        public void Configure(EntityTypeBuilder<DocumentMedical> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Titre)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(d => d.CheminFichier)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.MimeType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.TypeDocument)
                   .HasConversion<int>();

            builder.HasIndex(d => d.DossierId);
            builder.HasIndex(d => d.TypeDocument);

            builder.HasOne(d => d.DossierMedical)
                   .WithMany(dm => dm.Documents)
                   .HasForeignKey(d => d.DossierId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(d => !d.EstSupprime);

            builder.Ignore(d => d.DomainEvents);
        }
    }
}