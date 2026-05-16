using Dossier_Medical_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dossier_Medical_microservice.Infrastructure.Configurations
{
    public class DiagnosticConfiguration
        : IEntityTypeConfiguration<Diagnostic>
    {
        public void Configure(EntityTypeBuilder<Diagnostic> builder)
        {
            builder.ToTable("Diagnostics");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.CodeCIM11)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(d => d.LibelleCIM11)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(d => d.Commentaire)
                   .HasMaxLength(1000);

            builder.Property(d => d.Type)
                   .HasConversion<int>();

            builder.HasIndex(d => d.CodeCIM11);

            builder.HasIndex(d => d.ConsultationId);

            builder.Ignore(d => d.DomainEvents);

            builder.HasOne(d => d.Consultation)
                   .WithMany(c => c.Diagnostics)
                   .HasForeignKey(d => d.ConsultationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}