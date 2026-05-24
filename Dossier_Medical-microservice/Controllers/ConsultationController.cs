using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dossier_Medical_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly ConsultationService _consultationService;

        public ConsultationController(ConsultationService service)
        {
            _consultationService = service;
        }

        /// POST /api/v1/dossiers/{id}/consultations
        [HttpPost("dossiers/{id:guid}/consultations")]
        public async Task<IActionResult> Creer(
            Guid id,
            [FromBody] CreerConsultationRequest req,
            CancellationToken ct)
        {
            var reqWithId = req with { DossierId = id };
            var result = await _consultationService.CreerConsultationAsync(reqWithId, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// GET /api/v1/dossiers/{id}/consultations
        [HttpGet("dossiers/{id:guid}/consultations")]
        public async Task<IActionResult> GetHistorique(Guid id, CancellationToken ct)
        {
            var result = await _consultationService.GetHistoriqueAsync(id, ct);
            return Ok(result);
        }

        /// GET /api/v1/consultations/{id}
        [HttpGet("consultations/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _consultationService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        /// PUT /api/v1/consultations/{id}/examen
        [HttpPut("consultations/{id:guid}/examen")]
        public async Task<IActionResult> UpdateExamen(
            Guid id,
            [FromBody] UpdateExamenCliniqueRequest req,
            CancellationToken ct)
        {
            await _consultationService.UpdateExamenCliniqueAsync(id, req, ct);
            return NoContent();
        }

        /// PUT /api/v1/consultations/{id}/cloturer
        [HttpPut("consultations/{id:guid}/cloturer")]
        public async Task<IActionResult> Cloturer(
            Guid id,
            [FromBody] CloturerConsultationRequest req,
            CancellationToken ct)
        {
            await _consultationService.CloturerConsultationAsync(id, req, ct);
            return NoContent();
        }


    }
}
