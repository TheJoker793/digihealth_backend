namespace Auth_microservice.Domain.Events;

public class UserLoggedInEvent
{
    public Guid UserId { get; }
    public string IpAddress { get; }
    public DateTime OccurredOn { get; }

    public UserLoggedInEvent(Guid userId, string ipAddress)
    {
        UserId = userId;
        IpAddress = ipAddress;
        OccurredOn = DateTime.UtcNow;
    }
}