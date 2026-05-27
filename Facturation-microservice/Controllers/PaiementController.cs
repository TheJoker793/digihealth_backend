using Facturation_microservice.Application.DTOs.Requests;
using Facturation_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facturation_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaiementController : ControllerBase
    {
        private readonly FacturationService _service;

        public PaiementController(FacturationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Enregistrer(EnregistrerPaiementRequest request)
        {
            await _service.EnregistrerPaiementAsync(request);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetByFacture(Guid factureId)
        {
            var result = await _service.GetHistoriquePaiementsAsync(factureId);
            return Ok(result);
        }
    }
}
