using FluentValidation;
using Prescription_microservice.Application.DTOs.Requests;

namespace Prescription_microservice.Application.Validators
{
    public class CreerPrescriptionValidator : AbstractValidator<CreerPrescriptionRequest>
    {
        public CreerPrescriptionValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Le patientId est requis.");

            RuleFor(x => x.ValiditeJours)
                .InclusiveBetween(1, 365)
                .WithMessage("La validité doit être comprise entre 1 et 365 jours.");

            // Vérification logique côté domaine/service pour au moins une ligne
        }
    }
}
