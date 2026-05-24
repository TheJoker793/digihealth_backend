using FluentValidation;
using Prescription_microservice.Application.DTOs.Requests;

namespace Prescription_microservice.Application.Validators
{
    public class ContournerInteractionValidator : AbstractValidator<ContournerInteractionRequest>
    {
        public ContournerInteractionValidator()
        {
            RuleFor(x => x.Justification)
                .NotEmpty().WithMessage("La justification est obligatoire.")
                .MinimumLength(20).WithMessage("La justification doit contenir au moins 20 caractères.");

            RuleFor(x => x.MedecinId)
                .NotEmpty().WithMessage("Le medecinId est requis.");

            RuleFor(x => x.InteractionId)
                .NotEmpty().WithMessage("L'InteractionId est requis.");
        }
    }
}
