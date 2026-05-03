namespace Patient_microservice.Domain.Events
{
    public record PatientCreatedEvent(Guid PatientId, string Nom, string Prenom, DateOnly DateNaissance);
}
