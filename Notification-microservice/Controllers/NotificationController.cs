using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;

namespace Notification_microservice.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _service;
    private readonly IValidator<EnvoyerNotificationRequest> _validator;

    public NotificationController(
        NotificationService service,
        IValidator<EnvoyerNotificationRequest> validator)
    {
        _service   = service;
        _validator = validator;
    }

    // ── GET /api/v1/notification/{id} ────────────────────────
    /// <summary>Récupère une notification avec son historique d'envoi.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(
        Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return Ok(result);
    }

    // ── GET /api/v1/notification/destinataire/{id} ───────────
    /// <summary>Liste les notifications d'un destinataire (100 dernières).</summary>
    [HttpGet("destinataire/{destinataireId:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetByDestinataire(
        Guid destinataireId, CancellationToken ct)
    {
        var result = await _service.GetByDestinataireAsync(destinataireId, ct);
        return Ok(result);
    }

    // ── POST /api/v1/notification/envoyer ────────────────────
    /// <summary>Envoie une notification manuelle (usage admin / tests).</summary>
    [HttpPost("envoyer")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Envoyer(
        [FromBody] EnvoyerNotificationRequest request,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.EnvoyerAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // ── PUT /api/v1/notification/{id}/annuler ────────────────
    /// <summary>Annule une notification non encore envoyée.</summary>
    [HttpPut("{id:guid}/annuler")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Annuler(
        Guid id,
        [FromBody] AnnulerNotificationRequest request,
        CancellationToken ct)
    {
        await _service.AnnulerAsync(id, request.Motif, ct);
        return NoContent();
    }
}

// DTO local simple
public record AnnulerNotificationRequest(string Motif);
