using Dossier_Medical_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Dossier_Medical_microservice.Application.Validators
{
    public class CreerConsultationValidator : AbstractValidator<CreerConsultationRequest>
    {
        public CreerConsultationValidator()
        {
            RuleFor(x => x.DossierId)
                .NotEmpty().WithMessage("L'identifiant du dossier est obligatoire.");

            RuleFor(x => x.Motif)
                .NotEmpty().WithMessage("Le motif de consultation est obligatoire.")
                .MinimumLength(3).WithMessage("Le motif doit contenir au moins 3 caractères.");

            RuleFor(x => x.TypeConsultation)
                .IsInEnum().WithMessage("Type de consultation invalide.");
        }
    }
}
