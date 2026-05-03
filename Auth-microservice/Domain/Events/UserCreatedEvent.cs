using Auth_microservice.Domain.Enums;

public class UserCreatedEvent
{
    public Guid UserId { get; }
    public Role Role { get; }

    public UserCreatedEvent(Guid userId, Role role)
    {
        UserId = userId;
        Role = role;
    }
}