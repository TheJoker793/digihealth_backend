using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rendez_vous_microservice.Domain.Entities;
using Rendez_Vous_microservice.Application.Services;

namespace Rendez_Vous_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurrenceController : ControllerBase
    {
        private readonly RecurrenceService _service;

        public RecurrenceController(RecurrenceService service)
        {
            _service = service;
        }

        // 🔹 POST /api/v1/recurrences
        [HttpPost]
        public async Task<IActionResult> Creer([FromBody] RegleRecurrence regle)
        {
            var created = await _service.CreerRecurrence(regle);
            return CreatedAtAction(nameof(GetByMedecin), new { medecinId = created.MedecinId }, created);
        }
        // 🔹 GET /api/v1/recurrences/{medecinId}
        [HttpGet("{medecinId:guid}")]
        public async Task<IActionResult> GetByMedecin(Guid medecinId)
        {
            var regles = await _service.GetByMedecin(medecinId);
            return Ok(regles);
        }

        // 🔹 DELETE /api/v1/recurrences/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Supprimer(Guid id)
        {
            await _service.Supprimer(id);
            return NoContent();
        }
    }
}
