using System;
namespace Rendez_vous_microservice.Domain.Events
{
    public abstract class BaseDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
