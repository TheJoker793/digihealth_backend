namespace Statistique_microservice.Application.DTOs.Responses
{
    public record DashboardResponse(
    TableauDeBordResponse Tableau,
    IEnumerable<KPIResponse> KPIs,
    IEnumerable<SnapshotResponse> Tendance,    // N dernières semaines
    DateTimeOffset CalculeA
);
}
