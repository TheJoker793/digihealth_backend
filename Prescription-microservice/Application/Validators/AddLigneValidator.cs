using FluentValidation;
using Prescription_microservice.Application.DTOs.Requests;

namespace Prescription_microservice.Application.Validators
{
    public class AddLigneValidator : AbstractValidator<AddLigneRequest>
    {
        public AddLigneValidator()
        {
            RuleFor(x => x.MedicamentId)
                .NotEmpty().WithMessage("Le medicamentId est requis.");

            RuleFor(x => x.Posologie)
                .NotEmpty().WithMessage("La posologie est obligatoire.");

            RuleFor(x => x.DureeJours)
                .GreaterThan(0).WithMessage("La durée doit être supérieure à 0 jour.");

            RuleFor(x => x.Quantite)
                .GreaterThan(0).WithMessage("La quantité doit être supérieure à 0.");
        }
    }
}





