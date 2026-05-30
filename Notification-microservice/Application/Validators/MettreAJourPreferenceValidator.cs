using FluentValidation;
using Notification_microservice.Application.DTOs.Requests;

namespace Notification_microservice.Application.Validators
{
    public class MettreAJourPreferenceValidator : AbstractValidator<MettreAJourPreferenceRequest>
    {
        public MettreAJourPreferenceValidator()
        {
            RuleFor(x => x.CanauxActifs)
                .NotNull().WithMessage("CanauxActifs est obligatoire.")
                .Must(c => c.Length > 0).WithMessage("Au moins un canal doit être actif.");

            RuleFor(x => x)
                .Must(x =>
                {
                    if (x.HeureDebut == null && x.HeureFin == null) return true;
                    if (x.HeureDebut == null || x.HeureFin == null) return false;
                    return TimeOnly.TryParse(x.HeureDebut, out var debut)
                        && TimeOnly.TryParse(x.HeureFin, out var fin)
                        && fin > debut;
                })
                .WithMessage("HeureDebut et HeureFin doivent être valides et HeureFin > HeureDebut.");
        }
    }
}
