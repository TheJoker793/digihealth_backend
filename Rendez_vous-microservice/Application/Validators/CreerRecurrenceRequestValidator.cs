using FluentValidation;
using Rendez_vous_microservice.Application.DTOs.Requests;

namespace Rendez_vous_microservice.Application.Validators
{
    public class CreerRecurrenceRequestValidator : AbstractValidator<CreerRecurrenceRequest>
    {
        public CreerRecurrenceRequestValidator()
        {
            RuleFor(x => x.JoursSemaineBits)
                .Must(bits => bits != 0)
                .WithMessage("Au moins un jour de la semaine doit être sélectionné.");

            RuleFor(x => x.HeureFin)
                .GreaterThan(x => x.HeureDebut)
                .WithMessage("L'heure de fin doit être après l'heure de début.");
        }
    }
}
