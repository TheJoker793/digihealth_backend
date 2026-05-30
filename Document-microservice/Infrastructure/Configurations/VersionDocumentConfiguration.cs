using Document_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Document_microservice.Infrastructure.Configurations
{
    public class VersionDocumentConfiguration : IEntityTypeConfiguration<VersionDocument>
    {
        public void Configure(EntityTypeBuilder<VersionDocument> builder)
        {
            builder.ToTable("VersionsDocument");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.CheminFichier)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(v => v.NomFichier)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.Format)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(v => v.ChecksumSha256)
                .IsRequired()
                .HasMaxLength(64);

            builder.HasIndex(v => new { v.DocumentMedicalId, v.NumeroVersion }).IsUnique();
            builder.HasIndex(v => new { v.DocumentMedicalId, v.EstActive });

            // MetadonneeFichier — Value Object stocké inline (OwnsOne)
            builder.OwnsOne(v => v.Metadonnees, m =>
            {
                m.Property(x => x.Auteur).HasMaxLength(100).HasColumnName("Meta_Auteur");
                m.Property(x => x.Langue).HasMaxLength(10).HasColumnName("Meta_Langue");
                m.Property(x => x.NbPages).HasColumnName("Meta_NbPages");
                m.Property(x => x.DateCreationFichier).HasColumnName("Meta_DateCreation");

                // string[] stocké en JSON dans une colonne
                m.Property(x => x.MotsCles)
                    .HasColumnName("Meta_MotsCles")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
            });

            builder.Ignore(v => v.DomainEvents);
        }

    }
}
