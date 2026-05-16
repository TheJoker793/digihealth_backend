namespace Dossier_Medical_microservice.Application.DTOs.Responses
{
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int Total,
        int Page,
        int PageSize
    );
}
