using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.Services;
using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "MedecinOnly")]
public class RapportController : ControllerBase
{
    private readonly RapportService _service;
    private readonly IValidator<GenererRapportRequest> _validator;

    public RapportController(
        RapportService service,
        IValidator<GenererRapportRequest> validator)
    {
        _service   = service;
        _validator = validator;
    }

    // ── GET /api/v1/rapport/{id} ─────────────────────────────
    /// <summary>Récupère un rapport par son identifiant.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return Ok(result);
    }

    // ── GET /api/v1/rapport/cabinet/{cabinetId} ──────────────
    /// <summary>Liste les rapports d'un cabinet, filtrables par type.</summary>
    [HttpGet("cabinet/{cabinetId:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetByCabinet(
        Guid cabinetId,
        [FromQuery] TypeRapport? type,
        CancellationToken ct)
    {
        var result = await _service.GetByCabinetAsync(cabinetId, type, ct);
        return Ok(result);
    }

    // ── POST /api/v1/rapport/generer ─────────────────────────
    /// <summary>Génère un rapport immédiatement ou le planifie.</summary>
    [HttpPost("generer")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Generer(
        [FromBody] GenererRapportRequest request,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.GenererAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // ── GET /api/v1/rapport/{id}/exporter ────────────────────
    /// <summary>Télécharge le rapport en PDF ou Excel.</summary>
    [HttpGet("{id:guid}/exporter")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Exporter(
        Guid id,
        [FromQuery] string format = "pdf",
        CancellationToken ct = default)
    {
        if (format.ToLower() is not "pdf" and not "xlsx")
            return BadRequest("Format invalide. Valeurs acceptées : pdf, xlsx.");

        var (contenu, nomFichier, contentType) =
            await _service.ExporterAsync(id, format, ct);

        return File(contenu, contentType, nomFichier);
    }

    // ── PUT /api/v1/rapport/{id}/annuler ─────────────────────
    /// <summary>Annule un rapport planifié ou en attente.</summary>
    [HttpPut("{id:guid}/annuler")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Annuler(
        Guid id,
        [FromBody] AnnulerRapportRequest request,
        CancellationToken ct)
    {
        await _service.AnnulerAsync(id, request.Motif, ct);
        return NoContent();
    }
}

public record AnnulerRapportRequest(string Motif);
