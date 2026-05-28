using Document_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Document_microservice.Application.Validators
{
    public class AjouterVersionValidator : AbstractValidator<AjouterVersionRequest>
    {
        private const long TailleMaxOctets = 50 * 1024 * 1024;

        public AjouterVersionValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty().WithMessage("DocumentId obligatoire.");

            RuleFor(x => x.Fichier)
                .NotNull().WithMessage("Le fichier est obligatoire.")
                .Must(f => f.Length > 0).WithMessage("Le fichier est vide.")
                .Must(f => f.Length <= TailleMaxOctets)
                    .WithMessage("Le fichier dépasse la limite de 50 Mo.");
        }
    }
}
