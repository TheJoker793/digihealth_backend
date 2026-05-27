using Facturation_microservice.Domain.Entities;
using QuestPDF.Fluent;
using Facturation_microservice.Domain.Interfaces;

namespace Facturation_microservice.Infrastructure.Services
{
    public class QuestPdfService : IPdfService
    {
        public Task<byte[]> GenererFacturePdfAsync(Facture facture)
        {
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header().Text($"FACTURE {facture.NumeroFacture}")
                        .FontSize(20).Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Patient: {facture.PatientId}");
                        col.Item().Text($"Date: {facture.DateFacture}");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Désignation");
                                header.Cell().Text("PU");
                                header.Cell().Text("Qté");
                            });

                            foreach (var ligne in facture.LignesFacture)
                            {
                                table.Cell().Text(ligne.Designation);
                                table.Cell().Text(ligne.PrixUnitaire.ToString());
                                table.Cell().Text(ligne.Quantite.ToString());
                            }
                        });
                    });
                });
            });

            return Task.FromResult(pdf.GeneratePdf());
        }
    }
}