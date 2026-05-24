using Prescription_microservice.Domain.Enums;

namespace Prescription_microservice.Domain.Events
{
    public record InteractionDetecteeEvent(
        Guid PrescriptionId,
        Guid PatientId,
        string MedicamentA,
        string MedicamentB,
        SeveriteInteraction Severite,
        DateTimeOffset OccurredAt)
    {
        public InteractionDetecteeEvent(Guid prescriptionId, Guid patientId,
            string medA, string medB, SeveriteInteraction severite)
            : this(prescriptionId, patientId, medA, medB, severite, DateTimeOffset.UtcNow) { }
    }
}
