using Auth_microservice.DTOs.Requests;
using Auth_microservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Login, request.Password);
            return Ok(result);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _authService.RefreshAsync(request.RefreshToken);
            return Ok(result);
        }

        // =========================
        // LOGOUT
        // =========================
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _authService.LogoutAsync(request.RefreshToken);
            return NoContent();
        }
        // =========================
        // USER INFO
        // =========================
        [HttpGet("userinfo")]
        public IActionResult UserInfo()
        {
            return Ok(new { message = "secured endpoint" });
        }
    }
}