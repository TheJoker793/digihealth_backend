using FluentValidation;
using Statistique_microservice.Application.DTOs.Requests;

namespace Statistique_microservice.Application.Validators
{
    public class PersonnaliserTableauValidator : AbstractValidator<PersonnaliserTableauRequest>
    {
        public PersonnaliserTableauValidator()
        {
            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom est obligatoire.")
                .MaximumLength(100);

            RuleFor(x => x.KPIsAffiches)
                .NotEmpty().WithMessage("Au moins un KPI doit être sélectionné.")
                .Must(k => k.Length <= 12).WithMessage("Maximum 12 KPIs par tableau.");

            RuleFor(x => x.NbSemainesTendance)
                .InclusiveBetween(4, 52)
                .WithMessage("La fenêtre de tendance doit être entre 4 et 52 semaines.");
        }
    }
}
