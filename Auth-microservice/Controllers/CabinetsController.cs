using Auth_microservice.DTOs.Requests;
using Auth_microservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth_microservice.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CabinetsController : ControllerBase
    {
        private readonly CabinetService _cabinetService;

        public CabinetsController(CabinetService cabinetService)
        {
            _cabinetService = cabinetService;
        }

        // =====================
        // CREATE
        // =====================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCabinetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cabinetService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // =====================
        // GET BY ID
        // =====================
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _cabinetService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Cabinet introuvable" });

            return Ok(result);
        }

        // =====================
        // GET ALL
        // =====================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cabinetService.GetAllAsync();
            return Ok(result);
        }

        // =====================
        // GET ACTIVE
        // =====================
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _cabinetService.GetActiveAsync();
            return Ok(result);
        }

        // =====================
        // UPDATE
        // =====================
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCabinetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var result = await _cabinetService.UpdateAsync(dto);
            return Ok(result);
        }

        // =====================
        // DELETE
        // =====================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _cabinetService.DeleteAsync(id);

            if (!success)
                return NotFound(new { message = "Cabinet introuvable" });

            return NoContent();
        }
    }
}