using Microsoft.AspNetCore.Mvc;
using Rendez_vous_microservice.Application.Services;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_vous_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RendezVousController : ControllerBase
    {
        private readonly RendezVousService _service;

        public RendezVousController(RendezVousService service)
        {
            _service = service;
        }

        // 🔹 POST /api/v1/rendezvous
        [HttpPost]
        public async Task<IActionResult> PrendreRdv([FromBody] RendezVous rdv)
        {
            var created = await _service.PrendreRdv(rdv);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // 🔹 GET /api/v1/rendezvous/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rdv = await _service.PrendreRdv(new RendezVous()); // ⚠️ ton service n’a pas encore GetById
            // Idéalement, ajoute GetById dans RendezVousService
            return rdv is null ? NotFound() : Ok(rdv);
        }

        // 🔹 GET /api/v1/rendezvous/patient/{patientId}
        [HttpGet("patient/{patientId:guid}")]
        public async Task<IActionResult> GetByPatient(Guid patientId)
        {
            // ⚠️ ton service n’a pas encore GetByPatient, à ajouter
            var historique = await _service.GetAgenda(patientId, DateTime.MinValue, DateTime.MaxValue);
            return Ok(historique);
        }

        // 🔹 PUT /api/v1/rendezvous/{id}/confirmer
        [HttpPut("{id:guid}/confirmer")]
        public async Task<IActionResult> Confirmer(Guid id)
        {
            await _service.Confirmer(id);
            return NoContent();
        }

        // 🔹 PUT /api/v1/rendezvous/{id}/annuler
        [HttpPut("{id:guid}/annuler")]
        public async Task<IActionResult> Annuler(Guid id, [FromBody] string raison)
        {
            await _service.Annuler(id, raison);
            return NoContent();
        }

        // 🔹 PUT /api/v1/rendezvous/{id}/reporter
        [HttpPut("{id:guid}/reporter")]
        public async Task<IActionResult> Reporter(Guid id, [FromBody] DateTime nouvelleDate)
        {
            await _service.Reporter(id, nouvelleDate);
            return NoContent();
        }

        // 🔹 PUT /api/v1/rendezvous/{id}/arrive
        [HttpPut("{id:guid}/arrive")]
        public async Task<IActionResult> MarquerArrive(Guid id)
        {
            await _service.MarquerArrive(id);
            return NoContent();
        }

        // 🔹 PUT /api/v1/rendezvous/{id}/termine
        [HttpPut("{id:guid}/termine")]
        public async Task<IActionResult> Terminer(Guid id)
        {
            // ⚠️ ton service n’a pas encore TerminerConsultation, à ajouter
            // Exemple :
            // await _service.Terminer(id);
            return NoContent();
        }
    }
}
