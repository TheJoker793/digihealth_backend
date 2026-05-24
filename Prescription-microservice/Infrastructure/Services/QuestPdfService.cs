using Prescription_microservice.Domain.Entities;
using Prescription_microservice.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Prescription_microservice.Infrastructure.Services
{
    public class QuestPdfService : IPdfService
    {
        public async Task<byte[]> GenererPrescriptionAsync(Prescription prescription, CancellationToken ct = default)
        {
            if (prescription == null)
                throw new ArgumentNullException(nameof(prescription));

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    // Header
                    page.Header()
                        .Row(row =>
                        {
                            row.ConstantItem(80).Image("Assets/logo.png");
                            row.RelativeItem()
                               .Column(col =>
                               {
                                   col.Item().Text("Ordonnance Médicale").FontSize(20).Bold();
                                   col.Item().Text($"Prescription N° {prescription.NumeroPrescription.Valeur}");
                                   col.Item().Text($"Date : {prescription.Date:dd/MM/yyyy}");
                               });
                        });

                    // Content
                    page.Content()
                        .Column(col =>
                        {
                            col.Spacing(10);
                            col.Item().Text($"Patient ID : {prescription.PatientId}");
                            col.Item().LineHorizontal(1);
                            col.Item().Text("Lignes de prescription :").Bold();

                            foreach (var ligne in prescription.Lignes)
                            {
                                col.Item().Text($"- {ligne.NomMedicament} ({ligne.DCI})");
                                col.Item().Text($"  Posologie : {ligne.Posologie} | Durée : {ligne.DureeJours} jours | Quantité : {ligne.Quantite}");
                                if (ligne.Renouvellement)
                                    col.Item().Text("  Renouvellement autorisé").Italic();
                            }
                        });

                    // Footer
                    page.Footer()
                        .AlignRight()
                        .Column(col =>
                        {
                            col.Item().Text("Signature du médecin").Italic();
                            col.Item().Text($"Dr. {prescription.MedecinId}").Bold();
                        });
                });
            });

            var pdfBytes = document.GeneratePdf();
            return await Task.FromResult(pdfBytes);
        }
    }
}
