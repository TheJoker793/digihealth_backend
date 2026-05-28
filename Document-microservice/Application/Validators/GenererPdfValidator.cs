using Document_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Document_microservice.Application.Validators
{
    public class GenererPdfValidator : AbstractValidator<GenererPdfRequest>
    {
        public GenererPdfValidator()
        {
            RuleFor(x => x.TemplateId)
                .NotEmpty().WithMessage("TemplateId obligatoire.");

            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId obligatoire.");

            RuleFor(x => x.MedecinId)
                .NotEmpty().WithMessage("MedecinId obligatoire.");

            RuleFor(x => x.Variables)
                .NotNull().WithMessage("Les variables sont obligatoires.");
        }
    }
}
