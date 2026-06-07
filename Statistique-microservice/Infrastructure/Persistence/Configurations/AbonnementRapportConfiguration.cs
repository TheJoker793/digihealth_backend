using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Infrastructure.Persistence.Configurations
{
    public class AbonnementRapportConfiguration : IEntityTypeConfiguration<AbonnementRapport>
    {
        public void Configure(EntityTypeBuilder<AbonnementRapport> b)
        {
            b.ToTable("AbonnementsRapport");
            b.HasKey(a => a.Id);
            b.Property(a => a.TypeRapport).HasConversion<string>().HasMaxLength(30);
            b.Property(a => a.Frequence).HasConversion<string>().HasMaxLength(15);

            // string[] Destinataires → séparé par point-virgule
            b.Property(a => a.Destinataires)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries))
                .HasMaxLength(2000);

            b.HasIndex(a => new { a.CabinetId, a.EstActif });
            b.HasIndex(a => new { a.EstActif, a.ProchainEnvoi });

            b.Ignore(a => a.DomainEvents);
        }
    }
}
