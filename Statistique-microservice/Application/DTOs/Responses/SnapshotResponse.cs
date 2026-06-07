namespace Statistique_microservice.Application.DTOs.Responses
{
    public record SnapshotResponse(
    Guid Id,
    Guid CabinetId,
    string DateSnapshot,                // "2024-05-30"
    int NbConsultations,
    int NbNouveauxPatients,
    int NbPatientsUniques,
    int NbRdvConfirmes,
    int NbRdvAnnules,
    int NbOrdonnances,
    decimal ChiffreAffaires,
    decimal TauxOccupation,
    decimal TicketMoyen,
    decimal TauxAnnulationRdv,
    bool EstConsolide,
    DateTimeOffset CreatedAt
);
}
