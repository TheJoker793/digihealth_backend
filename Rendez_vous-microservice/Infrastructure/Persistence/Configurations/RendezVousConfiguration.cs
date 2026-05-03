using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Infrastructure.Persistence.Configurations
{
    public class RendezVousConfiguration : IEntityTypeConfiguration<RendezVous>
    {
        public void Configure(EntityTypeBuilder<RendezVous> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.PatientId);
            builder.HasIndex(r => r.MedecinId);
            builder.HasIndex(r => r.DateHeure);
            builder.HasIndex(r => r.Statut);

            builder.Property(r => r.Motif)
                   .HasMaxLength(250);

            builder.Property(r => r.NoteSecretaire)
                   .HasMaxLength(500);
        }
    }
}
