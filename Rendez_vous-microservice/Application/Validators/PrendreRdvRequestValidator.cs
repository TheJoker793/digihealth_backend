using FluentValidation;
using Rendez_vous_microservice.Application.DTOs.Requests;

namespace Rendez_vous_microservice.Application.Validators
{
    public class PrendreRdvRequestValidator : AbstractValidator<PrendreRdvRequest>
    {
        public PrendreRdvRequestValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Le patientId est requis.");

            RuleFor(x => x.DateHeure)
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("La date du rendez-vous doit être dans le futur.");

            RuleFor(x => x.DureeMinutes)
                .InclusiveBetween(15, 120)
                .WithMessage("La durée doit être comprise entre 15 et 120 minutes.");
        }
    }
}
