
namespace Prescription_microservice.Domain.Events
{
    public record PrescriptionExpireEvent(
        Guid PrescriptionId,
        Guid PatientId,
        DateTimeOffset OccurredAt)
    {
        public PrescriptionExpireEvent(Guid prescriptionId, Guid patientId)
            : this(prescriptionId, patientId, DateTimeOffset.UtcNow) { }
    }
}
