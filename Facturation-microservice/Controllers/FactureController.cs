using Facturation_microservice.Application.DTOs.Requests;
using Facturation_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facturation_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactureController : ControllerBase
    {
        private readonly FacturationService _service;
        public FactureController(FacturationService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Creer([FromBody] CreerFactureRequest request)
        {
            var result = await _service.CreerFactureAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId)
        {
            var result = await _service.GetHistoriquePaiementsAsync(patientId);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _service.GetStatsAsync();
            return Ok(result);
        }

        [HttpPut("{id}/valider")]
        public async Task<IActionResult> Valider(Guid id)
        {
            await _service.ValiderAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/annuler")]
        public async Task<IActionResult> Annuler([FromBody] AnnulerFactureRequest request)
        {
            await _service.AnnulerAsync( request);
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            var pdf = await _service.GenererPdfAsync(id);

            return File(pdf, "application/pdf", $"facture-{id}.pdf");
        }



    }
}
