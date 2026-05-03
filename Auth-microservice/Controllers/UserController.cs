using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Enums;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Auth_microservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;

        public UserController(IUnitOfWork uow, IPasswordHasher hasher)
        {
            _uow = uow;
            _hasher = hasher;
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

            // Deactivate/Activate n'existent pas pour le rôle
            // → on expose via UpdateAsync directement
            // TODO : ajouter user.ChangeRole(request.Role) dans l'entité
            await _uow.Users.UpdateAsync(user, ct);
            await _uow.SaveChangesAsync(ct);

            return NoContent();
        }

        // POST api/v1/user/{id}/reset-password
        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(Guid id, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(id, ct);
            if (user is null)
                return NotFound(new { message = "Utilisateur introuvable." });

            // TODO : brancher IEmailService
            return NoContent();
        }
    }
}