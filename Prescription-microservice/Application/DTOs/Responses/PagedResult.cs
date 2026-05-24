namespace Prescription_microservice.Application.DTOs.Responses
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; init; } = new List<T>();

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public int TotalCount { get; init; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
