namespace Dossier_Medical_microservice.Domain.Interfaces
{
    public interface ICim11Service
    {
        Task<Cim11Result?> SearchAsync(string query, string lang = "en", CancellationToken ct = default);
        Task<Cim11Detail?> GetByCodeAsync(string code, CancellationToken ct = default);
    }
    public record Cim11Result(string Code, string Libelle, string? Definition);
    public record Cim11Detail(string Code, string Libelle, string? Definition,
        string? CodeParent, IEnumerable<string> Inclusions, IEnumerable<string> Exclusions);
}
