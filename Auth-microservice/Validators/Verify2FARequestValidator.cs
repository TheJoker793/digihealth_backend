using Auth_microservice.DTOs.Requests;
using FluentValidation;

namespace Auth_microservice.Validators
{
    public class Verify2FARequestValidator:AbstractValidator<Verify2FARequest>
    {
        public Verify2FARequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .Length(6).WithMessage("TOTP code must be exactly 6 digits")
                .Matches("^[0-9]{6}$").WithMessage("Code must contain only digits");
        }

    }
}
