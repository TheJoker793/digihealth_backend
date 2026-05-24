using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dossier_Medical_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdonnanceController : ControllerBase
    {
        private readonly OrdonnanceService _ordonnanceService;
        public OrdonnanceController(OrdonnanceService service)
        {
            _ordonnanceService = service;
        }
        /// POST /api/v1/consultations/{id}/ordonnances
        [HttpPost("consultations/{id:guid}/ordonnances")]
       
        public async Task<IActionResult> Create(
            Guid id,
            [FromBody] CreateOrdonnanceRequest req,
            CancellationToken ct)
        {
            var result = await _ordonnanceService.CreateOrdonnanceAsync(id, req, ct);
            return CreatedAtAction(nameof(GetByConsultation), new { id }, result);
        }

        /// GET /api/v1/consultations/{id}/ordonnances
        [HttpGet("consultations/{id:guid}/ordonnances")]
        public async Task<IActionResult> GetByConsultation(Guid id, CancellationToken ct)
        {
            var result = await _ordonnanceService.GetByConsultationAsync(id, ct);
            return Ok(result);
        }

        /// GET /api/v1/dossiers/{id}/ordonnances/actives
        [HttpGet("dossiers/{id:guid}/ordonnances/actives")]
        public async Task<IActionResult> GetActives(Guid id, CancellationToken ct)
        {
            // id = patientId pour cette route
            var result = await _ordonnanceService.GetActiveByPatientAsync(id, ct);
            return Ok(result);
        }

    }
}
