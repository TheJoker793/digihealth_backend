namespace Patient_microservice.Domain.Events
{
    public record PatientMergedEvent(Guid SourcePatientId, Guid TargetPatientId);
}
