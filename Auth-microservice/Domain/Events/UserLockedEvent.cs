namespace Auth_microservice.Domain.Events;

public class UserLockedEvent
{
    public Guid UserId { get; }
    public DateTime LockedUntil { get; }
    public DateTime OccurredOn { get; }

    public UserLockedEvent(Guid userId, DateTime lockedUntil)
    {
        UserId = userId;
        LockedUntil = lockedUntil;
        OccurredOn = DateTime.UtcNow;
    }
}