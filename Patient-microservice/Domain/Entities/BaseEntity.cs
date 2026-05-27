namespace Patient_microservice.Domain.Entities
{

    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        private readonly List<object> _domainEvents = new();

        public IReadOnlyCollection<object> DomainEvents => _domainEvents;

        protected BaseEntity()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public void AddDomainEvent(object domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}