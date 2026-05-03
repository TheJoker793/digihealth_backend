namespace Patient_microservice.Domain.Events
{
    public record PatientUpdatedEvent(Guid PatientId, string Nom, string Prenom, DateOnly DateNaissance);
}
