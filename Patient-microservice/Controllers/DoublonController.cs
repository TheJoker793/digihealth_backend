using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Dtos;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DoublonController : ControllerBase
    {
        private readonly DoublonService _service;

        public DoublonController(DoublonService service) => _service = service;

        [HttpGet("detect")]
        public async Task<IActionResult> Detect([FromQuery] string cin, [FromQuery] string passeport, [FromQuery] string nom, [FromQuery] DateOnly dateNaissance)
        {
            var exists = await _service.Detect(cin, passeport, nom, dateNaissance);
            return Ok(new { Doublon = exists });
        }

        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromBody] MergeRequest request)
        {
            await _service.Merge(request.SourceId, request.TargetId);
            return NoContent();
        }
    }
}
