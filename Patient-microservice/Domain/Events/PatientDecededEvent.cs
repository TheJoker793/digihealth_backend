namespace Patient_microservice.Domain.Events
{
    public record PatientDecededEvent(Guid PatientId, DateOnly DateDeces);
}
