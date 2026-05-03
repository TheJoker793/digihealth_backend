using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Configurations
{
    public class RegleRecurrenceConfiguration : IEntityTypeConfiguration<RegleRecurrence>
    {
        public void Configure(EntityTypeBuilder<RegleRecurrence> builder)
        {
            builder.HasKey(r => r.Id);

            // FK MedecinId (si tu as une entité Medecin dans ton modèle global)
            builder.HasIndex(r => r.MedecinId);

            builder.HasIndex(r => r.DateDebut);

            builder.Property(r => r.JoursSemaineBits)
                   .IsRequired();

            builder.Property(r => r.HeureDebut)
                   .IsRequired();

            builder.Property(r => r.HeureFin)
                   .IsRequired();
        }
    }
}
