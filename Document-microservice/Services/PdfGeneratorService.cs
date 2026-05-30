using Document_microservice.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Document_microservice.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public PdfGeneratorService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public Task<byte[]> GenererDepuisHtmlAsync(
            string contenuHtml,
            CancellationToken ct = default)
        {
            // ⚠️ QuestPDF ne rend pas HTML directement
            // → on le transforme en bloc texte simple (fallback propre)

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    page.Content()
                        .Text(contenuHtml)
                        .FontSize(10);
                });
            });

            var pdfBytes = document.GeneratePdf();

            return Task.FromResult(pdfBytes);
        }

        public Task<byte[]> GenererDepuisTemplateAsync(
            string nomTemplate,
            Dictionary<string, string> variables,
            CancellationToken ct = default)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header()
                        .Text($"Document : {nomTemplate}")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Column(col =>
                        {
                            col.Spacing(10);

                            foreach (var kv in variables)
                            {
                                col.Item()
                                   .Text($"{kv.Key} : {kv.Value}")
                                   .FontSize(12);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Généré automatiquement - ")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken1);

                            text.Span(DateTime.Now.ToString("dd/MM/yyyy"))
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken1);
                        });
                                    });
            });

            var pdfBytes = document.GeneratePdf();

            return Task.FromResult(pdfBytes);
        }
    }
}