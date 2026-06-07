namespace Statistique_microservice.Application.Consumers
{
    public record ConsultationTermineeEvent(
    Guid ConsultationId, Guid CabinetId, Guid PatientId,
    bool EstNouveauPatient, DateTimeOffset Date);
}
