using Auth_microservice.DTOs.Requests;
using FluentValidation;

namespace Auth_microservice.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Invalid role");

            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("CabinetId is required");
        }
    }
}
