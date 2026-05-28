namespace Document_microservice.Domain.Interfaces
{
    public interface IVirusScanService
    {
        Task<VirusScanResult> ScannerAsync(Stream contenu, CancellationToken ct = default);
    }

    public record VirusScanResult(bool EstSain, string? NomVirus = null);

}
