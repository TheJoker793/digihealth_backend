using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Infrastructure.Persistence.Configurations
{
    public class TableauDeBordConfiguration : IEntityTypeConfiguration<TableauDeBord>
    {
        public void Configure(EntityTypeBuilder<TableauDeBord> b)
        {
            b.ToTable("TableauxDeBord");
            b.HasKey(t => t.Id);
            b.Property(t => t.Nom).IsRequired().HasMaxLength(100);
            b.Property(t => t.PeriodeDefaut).HasConversion<string>().HasMaxLength(20);

            // TypeKPI[] → CSV
            b.Property(t => t.KPIsAffiches)
                .HasConversion(
                    v => string.Join(',', v.Select(k => k.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(Enum.Parse<TypeKPI>).ToArray())
                .HasMaxLength(500);

            b.HasIndex(t => new { t.ProprietaireId, t.EstParDefaut });
            b.Ignore(t => t.DomainEvents);
        }
    }
}
