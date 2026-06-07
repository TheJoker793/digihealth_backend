using FluentValidation;
using Statistique_microservice.Application.DTOs.Requests;

namespace Statistique_microservice.Application.Validators
{
    public class CreerTableauDeBordValidator : AbstractValidator<CreerTableauDeBordRequest>
    {
        public CreerTableauDeBordValidator()
        {
            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("CabinetId obligatoire.");

            RuleFor(x => x.ProprietaireId)
                .NotEmpty().WithMessage("ProprietaireId obligatoire.");

            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom est obligatoire.")
                .MaximumLength(100);

            RuleFor(x => x.PeriodeDefaut)
                .IsInEnum().WithMessage("PeriodeDefaut invalide.");
        }
    }
}
