using FluentValidation;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Application.Validators
{
    public class EnvoyerNotificationValidator : AbstractValidator<EnvoyerNotificationRequest>
    {
        public EnvoyerNotificationValidator()
        {
            RuleFor(x => x.DestinataireId)
                .NotEmpty().WithMessage("DestinataireId obligatoire.");

            RuleFor(x => x.TypeDestinataire)
                .NotEmpty()
                .Must(t => t is "Patient" or "Medecin")
                .WithMessage("TypeDestinataire doit être Patient ou Medecin.");

            RuleFor(x => x.TypeEvenement)
                .IsInEnum().WithMessage("TypeEvenement invalide.");

            RuleFor(x => x.Canal)
                .IsInEnum().WithMessage("Canal invalide.");

            // Validation croisée canal ↔ contact
            RuleFor(x => x.ContactEmail)
                .NotEmpty()
                .WithMessage("ContactEmail obligatoire pour le canal Email.")
                .When(x => x.Canal == CanalEnvoi.Email)
                .EmailAddress()
                .WithMessage("Email invalide.")
                .When(x => x.Canal == CanalEnvoi.Email && x.ContactEmail != null);

            RuleFor(x => x.ContactTelephone)
                .NotEmpty()
                .WithMessage("ContactTelephone obligatoire pour le canal SMS.")
                .When(x => x.Canal == CanalEnvoi.SMS)
                .Matches(@"^\+?[1-9]\d{7,14}$")
                .WithMessage("Numéro de téléphone invalide (format international attendu).")
                .When(x => x.Canal == CanalEnvoi.SMS && x.ContactTelephone != null);

            RuleFor(x => x.TokenFcm)
                .NotEmpty()
                .WithMessage("TokenFcm obligatoire pour le canal Push.")
                .When(x => x.Canal == CanalEnvoi.Push);

            RuleFor(x => x.Variables)
                .NotNull().WithMessage("Les variables sont obligatoires.");

            RuleFor(x => x.DateProgrammee)
                .Must(d => d == null || d > DateTimeOffset.UtcNow)
                .WithMessage("La date programmée doit être dans le futur.");
        }
    }
}
