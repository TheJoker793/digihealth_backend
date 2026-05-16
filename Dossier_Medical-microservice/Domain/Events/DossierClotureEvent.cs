namespace Dossier_Medical_microservice.Domain.Events
{
    public record DossierClotureEvent(Guid DossierId, Guid PatientId, DateTimeOffset OccurredAt)
    {
        public DossierClotureEvent(Guid dossierId, Guid patientId)
            : this(dossierId, patientId, DateTimeOffset.UtcNow) { }
    }
}
