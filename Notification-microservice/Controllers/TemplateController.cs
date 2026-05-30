using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification_microservice.Application.DTOs.Requests;
using Notification_microservice.Application.Services;
using Notification_microservice.Domain.Enums;

namespace Notification_microservice.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class TemplateController : ControllerBase
{
    private readonly TemplateService _service;
    private readonly IValidator<CreerTemplateRequest> _validator;

    public TemplateController(
        TemplateService service,
        IValidator<CreerTemplateRequest> validator)
    {
        _service   = service;
        _validator = validator;
    }

    // ── GET /api/v1/template/{id} ────────────────────────────
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return Ok(result);
    }

    // ── GET /api/v1/template?typeEvenement=RdvConfirme ───────
    /// <summary>Liste tous les templates d'un type d'événement.</summary>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetByEvenement(
        [FromQuery] TypeEvenement typeEvenement,
        CancellationToken ct)
    {
        var result = await _service.GetByEvenementAsync(typeEvenement, ct);
        return Ok(result);
    }

    // ── POST /api/v1/template ────────────────────────────────
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Creer(
        [FromBody] CreerTemplateRequest request,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.CreerAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // ── PUT /api/v1/template/{id} ────────────────────────────
    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> MettreAJour(
        Guid id,
        [FromBody] MettreAJourTemplateRequest request,
        CancellationToken ct)
    {
        var result = await _service.MettreAJourAsync(id, request, ct);
        return Ok(result);
    }

    // ── PUT /api/v1/template/{id}/desactiver ─────────────────
    [HttpPut("{id:guid}/desactiver")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Desactiver(Guid id, CancellationToken ct)
    {
        await _service.DesactiverAsync(id, ct);
        return NoContent();
    }

    // ── PUT /api/v1/template/{id}/activer ────────────────────
    [HttpPut("{id:guid}/activer")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Activer(Guid id, CancellationToken ct)
    {
        await _service.ActiverAsync(id, ct);
        return NoContent();
    }
}
