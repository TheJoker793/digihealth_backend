using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prescription_microservice.Application.DTOs.Requests;
using Prescription_microservice.Application.Services;
using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionService _service;

        public PrescriptionController(PrescriptionService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreerPrescriptionRequest prescription)
            => Ok(await _service.CreerAsync(prescription));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetHistorique(Guid patientId)
            => Ok(await _service.GetHistoriqueAsync(patientId));

        [HttpGet("patient/{patientId}/actives")]
        public async Task<IActionResult> GetActives(Guid patientId)
            => Ok(await _service.GetActivesAsync(patientId));

        [HttpPut("{id}/refuser")]
        public async Task<IActionResult> Refuser(Guid id, [FromBody] string motif)
        {
            var request = new RefuserRequest(id, motif);
            await _service.RefuserAsync(request);
            return Ok();
        }


        [HttpPut("{id}/annuler")]
        public async Task<IActionResult> Annuler(Guid id)
        {
            await _service.AnnulerAsync(id);
            return Ok();
        }


        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            var pdfBytes = await _service.GenererPdfAsync(id);
            return File(pdfBytes, "application/pdf", $"prescription-{id}.pdf");
        }
    }
}
