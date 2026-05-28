using Document_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Document_microservice.Application.Validators
{
    public class CreerPartageValidator : AbstractValidator<CreerPartageRequest>
    {
        private static readonly string[] TypesValides = ["Medecin", "Patient", "Externe"];

        public CreerPartageValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty().WithMessage("DocumentId obligatoire.");

            RuleFor(x => x.DestinataireId)
                .NotEmpty().WithMessage("DestinataireId obligatoire.");

            RuleFor(x => x.TypeDestinataire)
                .NotEmpty()
                .Must(t => TypesValides.Contains(t))
                .WithMessage("TypeDestinataire doit être : Medecin, Patient ou Externe.");

            RuleFor(x => x.DateExpiration)
                .Must(d => d == null || d > DateTime.UtcNow)
                .WithMessage("La date d'expiration doit être dans le futur.");

            RuleFor(x => x.LimiteAcces)
                .Must(l => l == null || l > 0)
                .WithMessage("La limite d'accès doit être > 0.");
        }
    }
}
