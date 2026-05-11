using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rendez_vous_microservice.Application.Services;

namespace Rendez_Vous_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly CreneauService _creneauService;
        private readonly RendezVousService _rendezVousService;

        public AgendaController(CreneauService creneauService, RendezVousService rendezVousService)
        {
            _creneauService = creneauService;
            _rendezVousService = rendezVousService;
        }

        // 🔹 GET /api/v1/agenda/{medecinId}
        [HttpGet("{medecinId:guid}")]
        public async Task<IActionResult> GetAgenda(Guid medecinId, [FromQuery] DateTime debut, [FromQuery] DateTime fin)
        {
            var agenda = await _rendezVousService.GetAgenda(medecinId, debut, fin);
            return Ok(agenda);
        }

        // 🔹 GET /api/v1/agenda/{medecinId}/disponibilites
        [HttpGet("{medecinId:guid}/disponibilites")]
        public async Task<IActionResult> GetDisponibilites(Guid medecinId, [FromQuery] DateTime dateDebut, [FromQuery] DateTime dateFin)
        {
            var dispo = await _creneauService.GetDisponibles(medecinId, dateDebut,dateFin);
            return Ok(dispo);
        }
    }
}
