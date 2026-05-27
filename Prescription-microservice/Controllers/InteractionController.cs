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

        // PUT /api/prescriptions/{id}/interactions/{iid}/contourner
        [HttpPut("{id}/interactions/{iid}/contourner")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "MedecinOnly")]
        public async Task<IActionResult> Contourner(Guid id, Guid iid, [FromBody] string justification)
        {
            var medecinIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("sub")?.Value
                                 ?? throw new InvalidOperationException("MedecinId introuvable dans le token.");

            var request = new ContournerInteractionRequest(
                id,
                iid,
                Guid.Parse(medecinIdClaim),
                justification
            );

            await _service.ContournerInteractionAsync(request);
            return Ok();
        }
    }
}
