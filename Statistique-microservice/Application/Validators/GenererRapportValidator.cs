using FluentValidation;
using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.Validators
{
    public class GenererRapportValidator : AbstractValidator<GenererRapportRequest>
    {
        public GenererRapportValidator()
        {
            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("CabinetId obligatoire.");

            RuleFor(x => x.TypeRapport)
                .IsInEnum().WithMessage("TypeRapport invalide.");

            RuleFor(x => x.TypePeriode)
                .IsInEnum().WithMessage("TypePeriode invalide.");

            // Dates obligatoires si période personnalisée
            When(x => x.TypePeriode == TypePeriode.Personnalise, () =>
            {
                RuleFor(x => x.DateDebut)
                    .NotEmpty().WithMessage("DateDebut obligatoire pour une période personnalisée.")
                    .Must(d => DateOnly.TryParse(d, out _))
                    .WithMessage("DateDebut invalide — format attendu : yyyy-MM-dd.");

                RuleFor(x => x.DateFin)
                    .NotEmpty().WithMessage("DateFin obligatoire pour une période personnalisée.")
                    .Must(d => DateOnly.TryParse(d, out _))
                    .WithMessage("DateFin invalide — format attendu : yyyy-MM-dd.");

                RuleFor(x => x)
                    .Must(x =>
                    {
                        if (!DateOnly.TryParse(x.DateDebut, out var debut)) return true;
                        if (!DateOnly.TryParse(x.DateFin, out var fin)) return true;
                        return fin >= debut;
                    })
                    .WithMessage("DateFin doit être >= DateDebut.");
            });

            RuleFor(x => x.DatePlanifiee)
                .Must(d => d == null || d > DateTimeOffset.UtcNow)
                .WithMessage("DatePlanifiee doit être dans le futur.");
        }
    }
}
