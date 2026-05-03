using Microsoft.AspNetCore.Mvc;
using Patient_microservice.Domain.Entities;
using Patient_microservice.Services;

namespace Patient_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouvertureSocialeController : ControllerBase
    {
        private readonly CouvertureSocialService _service;

        public CouvertureSocialeController(CouvertureSocialService service) => _service = service;

        [HttpGet("{patientId}")]
        public async Task<IActionResult> Get(Guid patientId) => Ok(await _service.Get(patientId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CouvertureSociale couverture)
        {
            await _service.Add(couverture);
            return Ok(couverture);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CouvertureSociale couverture)
        {
            couverture.Id = id;
            await _service.Update(couverture);
            return Ok(couverture);
        }
    }
}
