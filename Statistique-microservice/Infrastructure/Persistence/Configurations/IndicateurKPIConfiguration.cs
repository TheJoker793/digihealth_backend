using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Infrastructure.Persistence.Configurations
{
    public class IndicateurKPIConfiguration : IEntityTypeConfiguration<IndicateurKPI>
    {
        public void Configure(EntityTypeBuilder<IndicateurKPI> b)
        {
            b.ToTable("IndicateursKPI");
            b.HasKey(k => k.Id);
            b.Property(k => k.TypeKPI).HasConversion<string>().HasMaxLength(40);
            b.Property(k => k.Code).IsRequired().HasMaxLength(60);
            b.Property(k => k.Unite).HasMaxLength(30);
            b.Property(k => k.Valeur).HasPrecision(18, 4);
            b.Property(k => k.ValeurPrecedente).HasPrecision(18, 4);
            b.Property(k => k.VariationPourcentage).HasPrecision(8, 2);

            b.OwnsOne(k => k.Periode, p =>
            {
                p.Property(x => x.DateDebut).HasColumnName("Periode_DateDebut");
                p.Property(x => x.DateFin).HasColumnName("Periode_DateFin");
                p.Property(x => x.TypePeriode)
                    .HasConversion<string>().HasMaxLength(20)
                    .HasColumnName("Periode_Type");
            });

            b.HasIndex(k => new { k.CabinetId, k.TypeKPI });
            b.Ignore(k => k.DomainEvents);
        }
    }
}
