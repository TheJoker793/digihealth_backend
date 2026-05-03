using FluentValidation;
using Rendez_vous_microservice.Application.DTOs.Requests;

namespace Rendez_vous_microservice.Application.Validators
{
    public class CreerCreneauRequestValidator : AbstractValidator<CreerCreneauRequest>
    {
        public CreerCreneauRequestValidator()
        {
            RuleFor(x => x.Fin)
                .GreaterThan(x => x.Debut)
                .WithMessage("La fin du créneau doit être après le début.");

            // Exemple de règle pour éviter chevauchement (à compléter avec service)
            RuleFor(x => x)
                .Must(request => request.Debut < request.Fin)
                .WithMessage("Le créneau ne doit pas être invalide.");
        }
    }
}
