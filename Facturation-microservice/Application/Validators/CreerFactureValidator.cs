using Facturation_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Facturation_microservice.Application.Validators
{
    public class CreerFactureValidator: AbstractValidator<CreerFactureRequest>
    {
        public CreerFactureValidator()
        {
            RuleFor(x => x.PatientId).NotEmpty();

            RuleFor(x => x.Lignes)
                .NotEmpty()
                .WithMessage("Au moins une ligne est obligatoire");

            RuleFor(x => x.DateEcheance)
                .Must(d => d > DateOnly.FromDateTime(DateTime.UtcNow));
        }
    }
}
