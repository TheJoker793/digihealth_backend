using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prescription_microservice.Application.DTOs.Requests;
using Prescription_microservice.Application.Services;

namespace Prescription_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteractionController : ControllerBase
    {
        private readonly InteractionService _service;

        public InteractionController(InteractionService service)
        {
            _service = service;
        }

        // GET /api/v1/prescriptions/{id}/interactions
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var interactions = await _service.VerifierAsync(id);
            return Ok(interactions);
        }

        // PUT /api/v1/prescriptions/{id}/interactions/{iid}/contourner
        [HttpPut("{iid}/contourner")]
        public async Task<IActionResult> Contourner(Guid id, Guid iid, [FromBody] string justification)
        {
            // ⚠️ MedecinId peut être récupéré via le JWT (claims) ou passé en body
            var medecinIdClaim = User.FindFirst("sub")?.Value
                                 ?? throw new InvalidOperationException("MedecinId introuvable dans le token.");

            var request = new ContournerInteractionRequest(
                iid,
                Guid.Parse(medecinIdClaim),
                justification
            )
            {
                // PrescriptionId n’est pas dans ton record actuel, 
                // si besoin ajoute-le dans le DTO pour plus de cohérence
            };

            await _service.ContournerInteractionAsync(request);
            return Ok();
        }
    }
}
