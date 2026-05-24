using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prescription_microservice.Application.Services;
using Prescription_microservice.Domain.Entities;

namespace Prescription_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigneController : ControllerBase
    {
        private readonly PrescriptionService _service;

        public LigneController(PrescriptionService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Add(Guid id, [FromBody] LignePrescription ligne)
        {
            await _service.AjouterLigneAsync(id, ligne);
            return Ok();
        }
        // DELETE /api/v1/prescriptions/{id}/lignes/{lid}
        [HttpDelete("{lid}")]
        public async Task<IActionResult> Remove(Guid id, Guid lid)
        {
            await _service.SupprimerLigneAsync(id, lid);
            return Ok();
        }


    }
}
