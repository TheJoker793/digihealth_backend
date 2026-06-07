using FluentValidation;
using Statistique_microservice.Application.DTOs.Requests;

namespace Statistique_microservice.Application.Validators
{
    public class CreerAbonnementValidator : AbstractValidator<CreerAbonnementRequest>
    {
        public CreerAbonnementValidator()
        {
            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("CabinetId obligatoire.");

            RuleFor(x => x.CreePar)
                .NotEmpty().WithMessage("CreePar obligatoire.");

            RuleFor(x => x.TypeRapport)
                .IsInEnum().WithMessage("TypeRapport invalide.");

            RuleFor(x => x.Frequence)
                .IsInEnum().WithMessage("Fréquence invalide.");

            RuleFor(x => x.Destinataires)
                .NotEmpty().WithMessage("Au moins un destinataire est obligatoire.")
                .Must(d => d.Length <= 10).WithMessage("Maximum 10 destinataires.");

            RuleForEach(x => x.Destinataires)
                .EmailAddress().WithMessage("Adresse email invalide.");
        }
    }
}
