using FluentValidation;
using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Application.Validators
{
    public class GetKPIsValidator : AbstractValidator<GetKPIsRequest>
    {
        public GetKPIsValidator()
        {
            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("CabinetId obligatoire.");

            RuleFor(x => x.TypePeriode)
                .IsInEnum().WithMessage("TypePeriode invalide.");

            When(x => x.TypePeriode == TypePeriode.Personnalise, () =>
            {
                RuleFor(x => x.DateDebut)
                    .NotEmpty().WithMessage("DateDebut obligatoire.")
                    .Must(d => DateOnly.TryParse(d, out _))
                    .WithMessage("DateDebut invalide.");

                RuleFor(x => x.DateFin)
                    .NotEmpty().WithMessage("DateFin obligatoire.")
                    .Must(d => DateOnly.TryParse(d, out _))
                    .WithMessage("DateFin invalide.");
            });
        }
    }
}
