using FluentValidation;
using Notification_microservice.Application.DTOs.Requests;

namespace Notification_microservice.Application.Validators
{
    public class CreerTemplateValidator : AbstractValidator<CreerTemplateRequest>
    {
        private static readonly string[] LanguesAutorisees = ["fr", "ar", "en"];

        public CreerTemplateValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Le code est obligatoire.")
                .MaximumLength(100)
                .Matches(@"^[a-z0-9_]+$")
                .WithMessage("Le code doit être en minuscules, chiffres et underscores uniquement.");

            RuleFor(x => x.TypeEvenement)
                .IsInEnum().WithMessage("TypeEvenement invalide.");

            RuleFor(x => x.Canal)
                .IsInEnum().WithMessage("Canal invalide.");

            RuleFor(x => x.Langue)
                .NotEmpty()
                .Must(l => LanguesAutorisees.Contains(l))
                .WithMessage("Langue invalide. Langues autorisées : fr, ar, en.");

            RuleFor(x => x.SujetTemplate)
                .NotEmpty().WithMessage("Le sujet template est obligatoire.")
                .MaximumLength(200);

            RuleFor(x => x.CorpsTemplate)
                .NotEmpty().WithMessage("Le corps template est obligatoire.");

            RuleFor(x => x.Variables)
                .NotNull().WithMessage("La liste des variables est obligatoire.");
        }


    }
}
