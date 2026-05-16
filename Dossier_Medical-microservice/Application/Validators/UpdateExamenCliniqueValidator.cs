using Dossier_Medical_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Dossier_Medical_microservice.Application.Validators
{
    public class UpdateExamenCliniqueValidator : AbstractValidator<UpdateExamenCliniqueRequest>
    {
        public UpdateExamenCliniqueValidator()
        {
            When(x => x.Poids.HasValue, () =>
                RuleFor(x => x.Poids!.Value)
                    .InclusiveBetween(1, 300)
                    .WithMessage("Le poids doit être entre 1 et 300 kg."));

            When(x => x.Taille.HasValue, () =>
                RuleFor(x => x.Taille!.Value)
                    .InclusiveBetween(30, 250)
                    .WithMessage("La taille doit être entre 30 et 250 cm."));

            When(x => x.Pouls.HasValue, () =>
                RuleFor(x => x.Pouls!.Value)
                    .InclusiveBetween(20, 300)
                    .WithMessage("Le pouls doit être entre 20 et 300 bpm."));

            When(x => x.Temperature.HasValue, () =>
                RuleFor(x => x.Temperature!.Value)
                    .InclusiveBetween(30, 45)
                    .WithMessage("La température doit être entre 30 et 45°C."));
        }
    }
}
