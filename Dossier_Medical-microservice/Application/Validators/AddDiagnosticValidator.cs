using Dossier_Medical_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Dossier_Medical_microservice.Application.Validators
{
    public class AddDiagnosticValidator : AbstractValidator<AddDiagnosticRequest>
    {
        public AddDiagnosticValidator()
        {
            RuleFor(x => x.CodeCIM11)
                .NotEmpty().WithMessage("Le code CIM-11 est obligatoire.")
                .Matches(@"^[A-Z0-9]+(\.[A-Z0-9]+)*$")
                .WithMessage("Format code CIM-11 invalide (ex: 6A80, 6A80.0).");

            RuleFor(x => x.LibelleCIM11)
                .NotEmpty().WithMessage("Le libellé du diagnostic est obligatoire.")
                .MaximumLength(300);

            RuleFor(x => x.TypeDiagnostic)
                .IsInEnum().WithMessage("Type de diagnostic invalide.");
        }
    }
}
