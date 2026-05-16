using Dossier_Medical_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Dossier_Medical_microservice.Application.Validators
{
        public class OuvrirDossierValidator : AbstractValidator<OuvrirDossierRequest>
        {
            public OuvrirDossierValidator()
            {
            RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("L'identifiant du patient est obligatoire.");

            RuleFor(x => x.MedecinId)
                .NotEmpty().WithMessage("L'identifiant du médecin est obligatoire.");

            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("L'identifiant du cabinet est obligatoire.");

            RuleFor(x => x.Motif)
                .NotEmpty().WithMessage("Le motif est obligatoire.")
                .MinimumLength(3).WithMessage("Le motif doit contenir au moins 3 caractères.")
                .MaximumLength(500).WithMessage("Le motif ne peut pas dépasser 500 caractères.");
        }
        }
        }

