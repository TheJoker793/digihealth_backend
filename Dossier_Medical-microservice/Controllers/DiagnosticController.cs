using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dossier_Medical_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController : ControllerBase
    {
        private readonly DiagnosticService _diagnosticService;
        public DiagnosticController(DiagnosticService service)
        {
            _diagnosticService = service;
        }

        /// GET /api/v1/consultations/{id}/diagnostics
        [HttpGet("consultations/{id:guid}/diagnostics")]
        public async Task<IActionResult> GetByConsultation(Guid id, CancellationToken ct)
        {
            var result = await _diagnosticService.GetByConsultationAsync(id, ct);
            return Ok(result);
        }

        /// POST /api/v1/consultations/{id}/diagnostics
        [HttpPost("consultations/{id:guid}/diagnostics")]
       
        public async Task<IActionResult> Add(
            Guid id,
            [FromBody] AddDiagnosticRequest req,
            CancellationToken ct)
        {
            var result = await _diagnosticService.AddDiagnosticAsync(id, req, ct);
            return CreatedAtAction(nameof(GetByConsultation), new { id }, result);
        }

        /// PUT /api/v1/diagnostics/{did}/confirmer
        [HttpPut("diagnostics/{did:guid}/confirmer")]
        
        public async Task<IActionResult> Confirmer(Guid did, CancellationToken ct)
        {
            await _diagnosticService.ConfirmerAsync(did, ct);
            return NoContent();
        }

        /// DELETE /api/v1/diagnostics/{did}
        [HttpDelete("diagnostics/{did:guid}")]
       
        public async Task<IActionResult> Delete(Guid did, CancellationToken ct)
        {
            await _diagnosticService.DeleteAsync(did, ct);
            return NoContent();
        }



    }
}
