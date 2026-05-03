using Auth_microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth_microservice.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // =========================
        // TABLE
        // =========================
        builder.ToTable("RefreshTokens");

        // =========================
        // PRIMARY KEY
        // =========================
        builder.HasKey(t => t.Id);

        // =========================
        // TOKEN
        // =========================
        builder.Property(t => t.Token)
            .IsRequired();

        builder.HasIndex(t => t.Token)
            .IsUnique();

        // =========================
        // USER FK
        // =========================
        builder.Property(t => t.UserId)
            .IsRequired();

        builder.HasIndex(t => t.UserId);

        // =========================
        // EXPIRATION
        // =========================
        builder.Property(t => t.ExpiresAt)
            .IsRequired();

        builder.HasIndex(t => t.ExpiresAt);

        // =========================
        // DEVICE INFO
        // =========================
        builder.Property(t => t.DeviceInfo)
            .HasMaxLength(256);

        // =========================
        // STATE
        // =========================
        builder.Property(t => t.IsRevoked)
            .IsRequired();

        // =========================
        // RELATION (FIX IMPORTANT)
        // =========================
        builder.HasOne(t => t.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}