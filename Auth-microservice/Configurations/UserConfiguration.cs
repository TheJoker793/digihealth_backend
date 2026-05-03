using Auth_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth_microservice.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // =========================
        // TABLE
        // =========================
        builder.ToTable("Users");

        // =========================
        // PRIMARY KEY
        // =========================
        builder.HasKey(u => u.Id);

        // =========================
        // LOGIN (UNIQUE INDEX)
        // =========================
        builder.Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Login)
            .IsUnique();

        // =========================
        // PASSWORD
        // =========================
        builder.Property(u => u.HashedPassword)
            .IsRequired();

        // =========================
        // ROLE
        // =========================
        builder.Property(u => u.Role)
            .IsRequired();

        // =========================
        // CABINET
        // =========================
        builder.Property(u => u.CabinetId)
            .IsRequired();

        // =========================
        // TOTP SECRET
        // =========================
        builder.Property(u => u.TotpSecret)
            .HasMaxLength(512);

        // ⚠️ IMPORTANT:
        // Le chiffrement du TotpSecret doit être fait dans le service (Application layer)
    }
}