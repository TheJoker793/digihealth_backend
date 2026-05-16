using Dossier_Medical_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Dossier_Medical_microservice.Application.Validators
{
    public class CreateOrdonnanceValidator : AbstractValidator<CreateOrdonnanceRequest>
    {
        public CreateOrdonnanceValidator()
        {
            RuleFor(x => x.ValiditeJours)
                .InclusiveBetween(1, 365)
                .WithMessage("La validité doit être entre 1 et 365 jours.");

            RuleFor(x => x.Lignes)
                .NotEmpty().WithMessage("L'ordonnance doit contenir au moins un médicament.")
                .Must(l => l.Count() <= 20)
                .WithMessage("Une ordonnance ne peut pas contenir plus de 20 lignes.");

            RuleForEach(x => x.Lignes).ChildRules(ligne =>
            {
                ligne.RuleFor(l => l.NomMedicament)
                    .NotEmpty().WithMessage("Le nom du médicament est obligatoire.");
                ligne.RuleFor(l => l.Posologie)
                    .NotEmpty().WithMessage("La posologie est obligatoire.");
                ligne.RuleFor(l => l.DureeJours)
                    .GreaterThan(0).WithMessage("La durée doit être supérieure à 0.");
                ligne.RuleFor(l => l.Quantite)
                    .GreaterThan(0).WithMessage("La quantité doit être supérieure à 0.");
            });
        }
    }
}
