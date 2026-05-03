using Microsoft.EntityFrameworkCore;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Domain.Interfaces; // IEventPublisher
using System.Linq;

namespace Patient_microservice.Persistence
{
    public class PatientDbContext : DbContext
    {
        private readonly IEventPublisher _eventPublisher;

        // Constructeur normal (avec EventPublisher)
        public PatientDbContext(DbContextOptions<PatientDbContext> options, IEventPublisher eventPublisher)
            : base(options)
        {
            _eventPublisher = eventPublisher;
        }

        // Constructeur utilisé par la factory (sans EventPublisher)
        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        // DbSets (tables)
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PieceIdentite> PieceIdentites { get; set; }
        public DbSet<Antecedent> Antecedents { get; set; }
        public DbSet<CouvertureSociale> CouvertureSociales { get; set; }
        public DbSet<ContactUrgence> ContactsUrgences { get; set; }
        public DbSet<AssuranceComplementaire> AssuranceComplementaires { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Récupérer toutes les entités avec des DomainEvents
            var entitiesWithEvents = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            // Sauvegarde en base
            var result = await base.SaveChangesAsync(cancellationToken);

            // Publier les événements sur RabbitMQ
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _eventPublisher.PublishAsync(domainEvent);
                }
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration Patient
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nom).IsRequired();
                entity.Property(p => p.Prenom).IsRequired();

                // Owned entity MedecinTraitant
                entity.OwnsOne(p => p.MedecinTraitant, mt =>
                {
                    mt.Property(m => m.Nom).IsRequired();
                    mt.Property(m => m.Prenom).IsRequired();
                    mt.Property(m => m.Specialite).IsRequired();
                    mt.Property(m => m.Telephone);
                    mt.Property(m => m.NumOrdre);
                    mt.Property(m => m.Adresse);
                });
            });

            // Configuration PieceIdentite
            modelBuilder.Entity<PieceIdentite>(entity =>
            {
                entity.HasKey(pi => pi.Id);
                entity.Property(pi => pi.PaysEmetteur).IsRequired();

                entity.HasOne(pi => pi.Patient)
                      .WithMany(p => p.PieceIdentites)
                      .HasForeignKey(pi => pi.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration CouvertureSociale
            modelBuilder.Entity<CouvertureSociale>(entity =>
            {
                entity.HasKey(cs => cs.Id);
                entity.Property(cs => cs.NumeroAffilie).IsRequired();
                entity.Property(cs => cs.DateDebut).IsRequired();
                entity.Property(cs => cs.DateFin).IsRequired();

                entity.HasOne(cs => cs.Patient)
                      .WithMany(p => p.CouvertureSociales)
                      .HasForeignKey(cs => cs.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration Antecedent
            modelBuilder.Entity<Antecedent>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.TypeAntecedent).IsRequired();
                entity.Property(a => a.Description).HasMaxLength(500);
            });

            // Configuration ContactUrgence
            modelBuilder.Entity<ContactUrgence>(entity =>
            {
                entity.HasKey(cu => cu.Id);
                entity.Property(cu => cu.Telephone).IsRequired();

                entity.HasOne(cu => cu.Patient)
                      .WithMany(p => p.ContactsUrgence)
                      .HasForeignKey(cu => cu.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration AssuranceComplementaire
            modelBuilder.Entity<AssuranceComplementaire>(entity =>
            {
                entity.HasKey(ac => ac.Id);
                entity.Property(ac => ac.NomAssureur).IsRequired();
                entity.Property(ac => ac.NumeroPolice).IsRequired();
                entity.Property(ac => ac.DateDebut).IsRequired();
                entity.Property(ac => ac.DateFin).IsRequired();
                entity.Property(ac => ac.TauxRemboursement).IsRequired();

                entity.HasOne(ac => ac.Patient)
                      .WithMany(p => p.AssurancesComplementaires)
                      .HasForeignKey(ac => ac.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
