using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.Services;
using Document_microservice.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly TemplateService _templateService;
        private readonly GenerationPdfService _pdfService;

        public TemplateController(
            TemplateService templateService,
            GenerationPdfService pdfService)
        {
            _templateService = templateService;
            _pdfService = pdfService;
        }

        /// <summary>
        /// Liste des templates
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await _templateService.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Récupérer un template
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _templateService.GetByIdAsync(
                id,
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Créer un template
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreerTemplateRequest template,
            CancellationToken cancellationToken)
        {
            var result = await _templateService.CreerAsync(
                template,
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Modifier un template
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] MettreAJourTemplateRequest template,
            CancellationToken cancellationToken)
        {
            await _templateService.MettreAJourAsync(
                id,
                template,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Supprimer un template
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _templateService.DesactiverAsync(
                id,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Générer un PDF depuis un template
        /// </summary>
        [HttpPost("{id:guid}/rendre")]
        public async Task<IActionResult> RenderPdf(
    Guid id,
    [FromBody] Dictionary<string, string> variables,
    CancellationToken cancellationToken)
        {
            var request = new GenererPdfRequest(
                TemplateId: id,
                PatientId: Guid.Empty,   // ⚠️ à adapter selon ton contexte réel
                MedecinId: Guid.Empty,
                CabinetId: Guid.Empty,
                Variables: variables,
                ConsultationId: null
            );

            var result = await _pdfService.GenererEtSauvegarderAsync(request, cancellationToken);

            return Ok(result);
        }
    }
}
