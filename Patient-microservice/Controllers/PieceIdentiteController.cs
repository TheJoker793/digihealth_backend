using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PieceIdentiteController : ControllerBase
    {
        private readonly PieceIdentiteService _service;

        public PieceIdentiteController(PieceIdentiteService service) => _service = service;

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId) => Ok(await _service.GetByPatient(patientId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PieceIdentite piece)
        {
            await _service.Add(piece);
            return Ok(piece);
        }

        
    }
}
