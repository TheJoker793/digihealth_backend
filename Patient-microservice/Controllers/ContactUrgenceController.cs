using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactUrgenceController : ControllerBase
    {
        private readonly ContactUrgenceService _service;

        public ContactUrgenceController(ContactUrgenceService service) => _service = service;

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId) => Ok(await _service.GetByPatient(patientId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ContactUrgence contact)
        {
            await _service.Add(contact);
            return Ok(contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ContactUrgence contact)
        {
            contact.Id = id;
            await _service.Update(contact);
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ContactUrgence contact)
        {
            await _service.Delete(contact);
            return NoContent();
        }

        
    }
}
