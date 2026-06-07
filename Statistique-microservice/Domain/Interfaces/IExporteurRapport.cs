using Statistique_microservice.Domain.Entities;

namespace Statistique_microservice.Domain.Interfaces
{
    public interface IExporteurRapport
    {
        Task<byte[]> ExporterPdfAsync(
            RapportStatistique rapport,
            CancellationToken ct = default);

        Task<byte[]> ExporterExcelAsync(
            RapportStatistique rapport,
            CancellationToken ct = default);
    }
}
