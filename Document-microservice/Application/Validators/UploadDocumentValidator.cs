using Document_microservice.Application.DTOs.Requests;
using FluentValidation;

namespace Document_microservice.Application.Validators
{
    public class UploadDocumentValidator : AbstractValidator<UploadDocumentRequest>
    {
        private static readonly string[] FormatsAutorises = ["application/pdf", "image/jpeg", "image/png", "application/dicom"];
        private const long TailleMaxOctets = 50 * 1024 * 1024; // 50 Mo

        public UploadDocumentValidator()
        {
            RuleFor(x => x.Titre)
                .NotEmpty().WithMessage("Le titre est obligatoire.")
                .MaximumLength(200).WithMessage("Le titre ne doit pas dépasser 200 caractères.");

            RuleFor(x => x.TypeDocument)
                .IsInEnum().WithMessage("Type de document invalide.");

            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId obligatoire.");

            RuleFor(x => x.MedecinId)
                .NotEmpty().WithMessage("MedecinId obligatoire.");

            RuleFor(x => x.CabinetId)
                .NotEmpty().WithMessage("CabinetId obligatoire.");

            RuleFor(x => x.Fichier)
                .NotNull().WithMessage("Le fichier est obligatoire.")
                .Must(f => f.Length > 0).WithMessage("Le fichier est vide.")
                .Must(f => f.Length <= TailleMaxOctets)
                    .WithMessage($"Le fichier dépasse la limite de 50 Mo.")
                .Must(f => FormatsAutorises.Contains(f.ContentType))
                    .WithMessage("Format non autorisé. Formats acceptés : PDF, JPEG, PNG, DICOM.");
        }
    }
}
