using Document_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Document_microservice.Application.Validators
{
    public class CreerTemplateValidator : AbstractValidator<CreerTemplateRequest>
    {
        public CreerTemplateValidator()
        {
            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom du template est obligatoire.")
                .MaximumLength(100);

            RuleFor(x => x.TypeDocument)
                .IsInEnum().WithMessage("Type de document invalide.");

            RuleFor(x => x.ContenuHtml)
                .NotEmpty().WithMessage("Le contenu HTML est obligatoire.");

            RuleFor(x => x.Variables)
                .NotNull().WithMessage("La liste des variables est obligatoire.");

            RuleFor(x => x.Version)
                .NotEmpty()
                .Matches(@"^\d+\.\d+$").WithMessage("Version invalide. Format attendu : 1.0, 1.1…");
        }
    }
}
