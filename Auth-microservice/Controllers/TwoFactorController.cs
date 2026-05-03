using Auth_microservice.DTOs.Requests;
using Auth_microservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TwoFactorController : ControllerBase
    {
        private readonly AuthService _authService;

        public TwoFactorController(AuthService authService)
        {
            _authService = authService;
        }

        // =========================
        // VERIFY 2FA
        // =========================
        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] Verify2FARequest request)
        {
            var token = await _authService.Verify2FAAsync(request.UserId, request.Code);
            return Ok(token);
        }
    }
}