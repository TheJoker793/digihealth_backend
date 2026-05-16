namespace Dossier_Medical_microservice.Domain.Events
{
    public record DossierOuvertEvent(Guid DossierId, Guid PatientId, Guid MedecinId, DateTimeOffset OccurredAt)
    {
        public DossierOuvertEvent(Guid dossierId, Guid patientId, Guid medecinId)
            : this(dossierId, patientId, medecinId, DateTimeOffset.UtcNow) { }
    }
}
