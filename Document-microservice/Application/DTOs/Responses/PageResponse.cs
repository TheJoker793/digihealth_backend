namespace Document_microservice.Application.DTOs.Responses
{
    public record PageResponse<T>(
    IEnumerable<T> Items,
    int Total,
    int Page,
    int PageSize
);
}
