using Facturation_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Facturation_microservice.Application.Validators
{
    public class AddLigneValidator : AbstractValidator<AddLigneRequest>
    {
        public AddLigneValidator()
        {
            RuleFor(x => x.Designation).NotEmpty();

            RuleFor(x => x.PrixUnitaire).GreaterThan(0);

            RuleFor(x => x.Quantite).GreaterThan(0);

            RuleFor(x => x.TauxRemboursement)
                .InclusiveBetween(0, 100);
        }
    }
}
