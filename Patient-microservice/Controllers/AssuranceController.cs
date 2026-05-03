using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AssuranceController : ControllerBase
    {
        private readonly AssuranceService _service;

        public AssuranceController(AssuranceService service) => _service = service;

        [HttpGet("{patientId}")]
        public async Task<IActionResult> Get(Guid patientId) => Ok(await _service.Get(patientId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AssuranceComplementaire assurance)
        {
            await _service.Add(assurance);
            return Ok(assurance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AssuranceComplementaire assurance)
        {
            assurance.Id = id;
            await _service.Update(assurance);
            return Ok(assurance);
        }
    }
}
