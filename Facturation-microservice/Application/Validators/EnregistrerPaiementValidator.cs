using Facturation_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Facturation_microservice.Application.Validators
{
    public class EnregistrerPaiementValidator : AbstractValidator<EnregistrerPaiementRequest>
    {
        public EnregistrerPaiementValidator()
        {
            RuleFor(x => x.Montant).GreaterThan(0);

            RuleFor(x => x.Mode).IsInEnum();

            RuleFor(x => x.Caissier).NotEmpty();
        }
    }
}
