using Hangfire;
using Microsoft.EntityFrameworkCore;
using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Enums;
using Prescription_microservice.Domain.Events;
using Prescription_microservice.Domain.Interfaces;
using Prescription_microservice.Infrastructure.Persistence;

namespace Prescription_microservice.Infrastructure.Jobs
{
    public class HangfireExpirationJob
    {
        private readonly PrescriptionDbContext _context;
        private readonly IEventPublisher _eventPublisher;

        public HangfireExpirationJob(PrescriptionDbContext context, IEventPublisher eventPublisher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        // Méthode appelée par Hangfire
        [AutomaticRetry(Attempts = 0)]
        public async Task RunAsync(CancellationToken ct = default)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var prescriptions = await _context.Prescriptions
                .Where(p => p.Statut == StatutPrescription.Validee &&
                            today > p.Date.AddDays(p.ValiditeJours))
                .ToListAsync(ct);

            foreach (var prescription in prescriptions)
            {
                prescription.MarquerExpiree();

                // Publier l’événement de domaine
                var evt = new PrescriptionExpireEvent(prescription.Id, prescription.PatientId);
                await _eventPublisher.PublishAsync(evt, ct);
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}
