using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientController(PatientService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> Create([FromBody] Patient patient )
        {
            await _service.Create(patient);
            return Ok(patient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var patient = await _service.GetById(id);
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string nom, [FromQuery] string prenom)
        {
            var patients = await _service.Search(nom, prenom);
            return Ok(patients);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Patient request)
        {
            var patient = await _service.GetById(id);
            if (patient == null) return NotFound();
            await _service.Update(patient);
            return Ok(patient);
        }

        

        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromBody] Patient request)
        {
            //await _service.Merge(request.SourceId, request.TargetId);
            return NoContent();
        }
    }
}
