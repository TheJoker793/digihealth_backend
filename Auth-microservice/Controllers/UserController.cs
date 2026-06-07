using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Enums;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.DTOs.Requests;
using Auth_microservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly UserService userService;
        private readonly IPasswordHasher _hasher;

        public UserController(IUnitOfWork uow, IPasswordHasher hasher,UserService service)
        {
            _uow = uow;
            _hasher = hasher;
            userService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync() 
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        // POST api/v1/user/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateUserRequest request,
            CancellationToken ct)
        {
            var login = request.Login.ToLowerInvariant();

            // ── Doublon ───────────────────────────────────────
            if (await _uow.Users.LoginExistsAsync(login, ct))
                return Conflict(new { message = "Login déjà utilisé." });

            // ── Hash mot de passe ─────────────────────────────
            var hashedPassword = _hasher.Hash(request.Password);

            // ── Créer l'entité Domain ─────────────────────────
            // "newUser" évite le conflit avec this.User (ClaimsPrincipal de ControllerBase)
            var newUser = Domain.Entities.User.Create(
                login,
                hashedPassword,
                request.Role,
                request.CabinetId
            );

            // ── Persister ─────────────────────────────────────
            await _uow.Users.AddAsync(newUser, ct);
            await _uow.SaveChangesAsync(ct);

            // ── Réponse ───────────────────────────────────────
            return CreatedAtAction(nameof(Create), new
            {
                id = newUser.Id,
                login = newUser.Login,
                role = newUser.Role.ToString(),
                cabinetId = newUser.CabinetId
            });
        }

        // PUT api/v1/user/{id}/disable
        [HttpPut("{id}/disable")]
        public async Task<IActionResult> Disable(Guid id, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(id, ct);
            if (user is null)
                return NotFound(new { message = "Utilisateur introuvable." });

            user.Deactivate();
            await _uow.Users.UpdateAsync(user, ct);
            await _uow.SaveChangesAsync(ct);

            return NoContent();
        }

        // PUT api/v1/user/{id}/role
        [HttpPut("{id}/role")]
                public async Task<IActionResult> ChangeRole(
            Guid id,
            [FromBody] ChangeRoleRequest request,
            CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(id, ct);
            if (user is null)
                return NotFound(new { message = "Utilisateur introuvable." });

            user.ChangeRole((Role)request.Role);

            await _uow.Users.UpdateAsync(user, ct);
            await _uow.SaveChangesAsync(ct);

            return NoContent();
        }

        // POST api/v1/user/{id}/reset-password
        [HttpPost("{id}/reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(
    Guid id,
    [FromBody] ChangePasswordRequest request,
    CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(id, ct);
            if (user is null)
                return NotFound(new { message = "Utilisateur introuvable." });

            if (!_hasher.Verify(request.CurrentPassword, user.HashedPassword))
                return BadRequest(new { message = "Mot de passe actuel incorrect." });

            var hashedNew = _hasher.Hash(request.NewPassword);
            user.ChangePassword(hashedNew);

            await _uow.Users.UpdateAsync(user, ct);
            await _uow.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}