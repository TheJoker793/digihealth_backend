using Facturation_microservice.Application.DTOs.Requests;
using Facturation_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facturation_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigneController : ControllerBase
    {
        private readonly FacturationService _service;

        public LigneController(FacturationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid factureId, AddLigneRequest request)
        {
            await _service.AddLigne(factureId, request);
            return Ok();
        }

        [HttpDelete("{ligneId}")]
        public async Task<IActionResult> Delete(Guid factureId, Guid ligneId)
        {
            await _service.RemoveLigne(factureId, ligneId);
            return NoContent();
        }
    }
}
