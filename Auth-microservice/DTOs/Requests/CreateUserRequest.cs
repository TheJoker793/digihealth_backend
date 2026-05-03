using Auth_microservice.Domain.Enums;

namespace Auth_microservice.DTOs.Requests
{
    public record CreateUserRequest(
        string Login,
        string Password,
        Role Role,
        string CabinetId
    );
}
