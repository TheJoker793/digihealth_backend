
namespace Prescription_microservice.Domain.Events
{
    public record PrescriptionRefuseeEvent(
        Guid PrescriptionId,
        Guid PatientId,
        string Motif,
        DateTimeOffset OccurredAt)
    {
        public PrescriptionRefuseeEvent(Guid prescriptionId, Guid patientId, string motif)
            : this(prescriptionId, patientId, motif, DateTimeOffset.UtcNow) { }
    }
}
