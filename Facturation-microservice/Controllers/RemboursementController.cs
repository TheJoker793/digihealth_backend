using Facturation_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facturation_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemboursementController : ControllerBase
    {
        private readonly RemboursementService _service;

        public RemboursementController(RemboursementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid factureId)
        {
            var result = await _service.GetByFactureAsync(factureId);
            return Ok(result);
        }

        [HttpPost("soumettre")]
        public async Task<IActionResult> Soumettre(Guid factureId)
        {
            await _service.SoumettreALaCaisse(factureId);
            return Accepted();
        }
    }
}
