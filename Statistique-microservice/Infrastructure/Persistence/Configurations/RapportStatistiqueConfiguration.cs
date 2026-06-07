using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Infrastructure.Persistence.Configurations
{
    public class RapportStatistiqueConfiguration : IEntityTypeConfiguration<RapportStatistique>
    {
        public void Configure(EntityTypeBuilder<RapportStatistique> b)
        {
            b.ToTable("RapportsStatistique");
            b.HasKey(r => r.Id);
            b.Property(r => r.Numero).IsRequired().HasMaxLength(40);
            b.HasIndex(r => r.Numero).IsUnique();
            b.Property(r => r.TypeRapport).HasConversion<string>().HasMaxLength(30);
            b.Property(r => r.Statut).HasConversion<string>().HasMaxLength(15);
            b.Property(r => r.DonneesJson).HasColumnType("nvarchar(max)");
            b.Property(r => r.CheminFichierPdf).HasMaxLength(500);
            b.Property(r => r.CheminFichierExcel).HasMaxLength(500);
            b.Property(r => r.MessageErreur).HasMaxLength(1000);

            // PeriodeAnalyse — Value Object inline (OwnsOne)
            b.OwnsOne(r => r.Periode, p =>
            {
                p.Property(x => x.DateDebut).HasColumnName("Periode_DateDebut");
                p.Property(x => x.DateFin).HasColumnName("Periode_DateFin");
                p.Property(x => x.TypePeriode)
                    .HasConversion<string>().HasMaxLength(20)
                    .HasColumnName("Periode_Type");
            });

            b.HasIndex(r => new { r.CabinetId, r.TypeRapport, r.Statut });
            b.HasIndex(r => new { r.Statut, r.DatePlanifiee });

            b.HasMany(r => r.Indicateurs)
                .WithOne()
                .HasForeignKey("RapportStatistiqueId")
                .OnDelete(DeleteBehavior.Cascade);

            b.Ignore(r => r.DomainEvents);
        }
    }
}
