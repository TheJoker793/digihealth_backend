using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dossier_Medical_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DossierController : ControllerBase
    {
        private readonly DossierService _dossierService;
        public DossierController(DossierService service)
        {
            _dossierService = service;
        }
        /// POST /api/v1/dossiers — ouvrir un dossier
        [HttpPost]
        public async Task<IActionResult> Ouvrir(
            [FromBody] OuvrirDossierRequest req,
            CancellationToken ct)
        {
            var result = await _dossierService.OuvrirDossierAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// GET /api/v1/dossiers/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _dossierService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        /// GET /api/v1/dossiers/patient/{patientId}
        [HttpGet("patient/{patientId:guid}")]
        public async Task<IActionResult> GetByPatient(Guid patientId, CancellationToken ct)
        {
            var result = await _dossierService.GetByPatientAsync(patientId, ct);
            return Ok(result);
        }

        /// GET /api/v1/dossiers/{id}/resume
        [HttpGet("{id:guid}/resume")]
        public async Task<IActionResult> GetResume(Guid id, CancellationToken ct)
        {
            var result = await _dossierService.GetResumeAsync(id, ct);
            return Ok(result);
        }

        /// PUT /api/v1/dossiers/{id}/cloturer
        [HttpPut("{id:guid}/cloturer")]
        public async Task<IActionResult> Cloturer(Guid id, CancellationToken ct)
        {
            await _dossierService.CloturerAsync(id, ct);
            return NoContent();
        }

        /// PUT /api/v1/dossiers/{id}/archiver
        [HttpPut("{id:guid}/archiver")]
        public async Task<IActionResult> Archiver(Guid id, CancellationToken ct)
        {
            await _dossierService.ArchiverAsync(id, ct);
            return NoContent();
        }
    }
}
