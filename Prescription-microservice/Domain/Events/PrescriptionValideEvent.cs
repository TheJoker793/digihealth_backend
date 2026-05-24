namespace Prescription_microservice.Domain.Events
{
    public record PrescriptionValideEvent(
        Guid PrescriptionId,
        Guid PatientId,
        Guid MedecinId,
        string NumeroPrescription,
        DateTimeOffset OccurredAt)
    {
        public PrescriptionValideEvent(Guid prescriptionId, Guid patientId, Guid medecinId, string numero)
            : this(prescriptionId, patientId, medecinId, numero, DateTimeOffset.UtcNow) { }
    }
}
