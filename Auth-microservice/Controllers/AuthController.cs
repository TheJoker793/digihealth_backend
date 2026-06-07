using Auth_microservice.DTOs.Requests;
using Auth_microservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUnitOfWork _uow;

        public AuthController(AuthService authService, IUnitOfWork uow)
        {
            _authService = authService;
            _uow = uow;
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
        [Authorize]
        public async Task<IActionResult> UserInfo(CancellationToken ct)
        {
            // ← remplace JwtRegisteredClaimNames.Sub par ClaimTypes.NameIdentifier
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine($"UserId from claims: {userId}");
            Console.WriteLine($"All claims: {string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");

            if (userId is null)
                return Unauthorized(new { message = "User ID not found in token" });

            var user = await _uow.Users.GetByIdAsync(Guid.Parse(userId), ct);
            if (user is null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                id = user.Id,
                login = user.Login,
                role = user.Role.ToString(),
                cabinetId = user.CabinetId,
                isActive = user.IsActive
            });
        }
    }
}