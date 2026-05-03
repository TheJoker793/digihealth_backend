using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AntecedentController : ControllerBase
    {
        private readonly AntecedentService _service;

        public AntecedentController(AntecedentService service) => _service = service;

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId) => Ok(await _service.GetByPatient(patientId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Antecedent antecedent)
        {
            await _service.Add(antecedent);
            return Ok(antecedent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Antecedent antecedent)
        {
            antecedent.Id = id;
            await _service.Update(antecedent);
            return Ok(antecedent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Antecedent ant)
        {
            await _service.Delete(ant);
            return NoContent();
        }
    }
}
