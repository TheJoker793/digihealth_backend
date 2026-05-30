using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;

namespace Notification_microservice.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PreferenceController : ControllerBase
{
    private readonly PreferenceService _service;
    private readonly IValidator<MettreAJourPreferenceRequest> _validator;

    public PreferenceController(
        PreferenceService service,
        IValidator<MettreAJourPreferenceRequest> validator)
    {
        _service   = service;
        _validator = validator;
    }

    // ── GET /api/v1/preference/{destinataireId}?type=Patient ─
    /// <summary>Récupère (ou crée par défaut) les préférences d'un destinataire.</summary>
    [HttpGet("{destinataireId:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get(
        Guid destinataireId,
        [FromQuery] string type,
        CancellationToken ct)
    {
        var result = await _service.GetOuCreerAsync(destinataireId, type, ct);
        return Ok(result);
    }

    // ── PUT /api/v1/preference/{destinataireId}?type=Patient ─
    /// <summary>Met à jour les préférences : canaux, plage horaire, langue.</summary>
    [HttpPut("{destinataireId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> MettreAJour(
        Guid destinataireId,
        [FromQuery] string type,
        [FromBody] MettreAJourPreferenceRequest request,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.MettreAJourAsync(destinataireId, type, request, ct);
        return Ok(result);
    }

    // ── POST /api/v1/preference/{destinataireId}/opt-out ─────
    /// <summary>Opt-out global — le destinataire refuse toutes les notifications.</summary>
    [HttpPost("{destinataireId:guid}/opt-out")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> OptOut(
        Guid destinataireId,
        [FromQuery] string type,
        CancellationToken ct)
    {
        await _service.OptOutAsync(destinataireId, type, ct);
        return NoContent();
    }

    // ── DELETE /api/v1/preference/{destinataireId}/opt-out ───
    /// <summary>Annule l'opt-out — le destinataire accepte de nouveau les notifications.</summary>
    [HttpDelete("{destinataireId:guid}/opt-out")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AnnulerOptOut(
        Guid destinataireId,
        [FromQuery] string type,
        CancellationToken ct)
    {
        await _service.AnnulerOptOutAsync(destinataireId, type, ct);
        return NoContent();
    }
}
