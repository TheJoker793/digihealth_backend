using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rendez_vous_microservice.Application.Services;
using Rendez_vous_microservice.Domain.Entities;

namespace Rendez_Vous_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreneauController : ControllerBase
    {
        private readonly CreneauService _service;

        public CreneauController(CreneauService service)
        {
            _service = service;
        }

        // 🔹 POST /api/v1/creneaux
        [HttpPost]
        public async Task<IActionResult> Creer([FromBody] Creneau creneau)
        {
            var created = await _service.CreerCreneau(creneau);
            return CreatedAtAction(nameof(Creer), new { id = created.Id }, created);
        }

        // 🔹 PUT /api/v1/creneaux/{id}/bloquer
        [HttpPut("{id:guid}/bloquer")]
        public async Task<IActionResult> Bloquer(Guid id)
        {
            await _service.Bloquer(id);
            return NoContent();
        }

        // 🔹 DELETE /api/v1/creneaux/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Supprimer(Guid id)
        {
            await _service.Supprimer(id);
            return NoContent();
        }
    }
}
