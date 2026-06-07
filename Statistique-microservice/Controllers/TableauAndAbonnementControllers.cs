using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.Services;

namespace Statistique_microservice.Controllers;

// ═══════════════════════════════════════════════════════════
// TableauDeBordController
// ═══════════════════════════════════════════════════════════
[ApiController]
[Route("api/v1/tableau-de-bord")]
[Authorize(Policy = "MedecinOnly")]
public class TableauDeBordController : ControllerBase
{
    private readonly TableauDeBordService _service;
    private readonly IValidator<CreerTableauDeBordRequest> _valCreer;
    private readonly IValidator<PersonnaliserTableauRequest> _valPerso;

    public TableauDeBordController(
        TableauDeBordService service,
        IValidator<CreerTableauDeBordRequest> valCreer,
        IValidator<PersonnaliserTableauRequest> valPerso)
    {
        _service  = service;
        _valCreer = valCreer;
        _valPerso = valPerso;
    }

    // ── GET /api/v1/tableau-de-bord/{id} ─────────────────────
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByProprietaireAsync(id, ct);
        return Ok(result);
    }

    // ── GET /api/v1/tableau-de-bord/{id}/dashboard ───────────
    /// <summary>
    /// Retourne le tableau de bord enrichi avec les KPIs calculés
    /// et les snapshots de tendance.
    /// </summary>
    [HttpGet("{id:guid}/dashboard")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetDashboard(Guid id, CancellationToken ct)
    {
        var result = await _service.GetDashboardAsync(id, ct);
        return Ok(result);
    }

    // ── GET /api/v1/tableau-de-bord/proprietaire/{id} ────────
    /// <summary>Liste tous les tableaux d'un propriétaire (médecin).</summary>
    [HttpGet("proprietaire/{proprietaireId:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetByProprietaire(
        Guid proprietaireId, CancellationToken ct)
    {
        var result = await _service.GetByProprietaireAsync(proprietaireId, ct);
        return Ok(result);
    }

    // ── POST /api/v1/tableau-de-bord ─────────────────────────
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Creer(
        [FromBody] CreerTableauDeBordRequest request,
        CancellationToken ct)
    {
        var validation = await _valCreer.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.CreerAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // ── PUT /api/v1/tableau-de-bord/{id} ─────────────────────
    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Personnaliser(
        Guid id,
        [FromBody] PersonnaliserTableauRequest request,
        CancellationToken ct)
    {
        var validation = await _valPerso.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.PersonnaliserAsync(id, request, ct);
        return Ok(result);
    }

    // ── DELETE /api/v1/tableau-de-bord/{id} ──────────────────
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        await _service.SupprimerAsync(id, ct);
        return NoContent();
    }
}

// ═══════════════════════════════════════════════════════════
// AbonnementController
// ═══════════════════════════════════════════════════════════
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "MedecinOnly")]
public class AbonnementController : ControllerBase
{
    private readonly AbonnementService _service;
    private readonly IValidator<CreerAbonnementRequest> _validator;

    public AbonnementController(
        AbonnementService service,
        IValidator<CreerAbonnementRequest> validator)
    {
        _service   = service;
        _validator = validator;
    }

    // ── GET /api/v1/abonnement/cabinet/{cabinetId} ───────────
    [HttpGet("cabinet/{cabinetId:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetByCabinet(Guid cabinetId, CancellationToken ct)
    {
        var result = await _service.GetByCabinetAsync(cabinetId, ct);
        return Ok(result);
    }

    // ── POST /api/v1/abonnement ──────────────────────────────
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Creer(
        [FromBody] CreerAbonnementRequest request,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.CreerAsync(request, ct);
        return CreatedAtAction(nameof(GetByCabinet),
            new { cabinetId = result.CabinetId }, result);
    }

    // ── PUT /api/v1/abonnement/{id} ──────────────────────────
    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> MettreAJour(
        Guid id,
        [FromBody] MettreAJourAbonnementRequest request,
        CancellationToken ct)
    {
        var result = await _service.MettreAJourAsync(id, request, ct);
        return Ok(result);
    }

    // ── PUT /api/v1/abonnement/{id}/activer ──────────────────
    [HttpPut("{id:guid}/activer")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Activer(Guid id, CancellationToken ct)
    {
        await _service.ActiverAsync(id, ct);
        return NoContent();
    }

    // ── PUT /api/v1/abonnement/{id}/desactiver ───────────────
    [HttpPut("{id:guid}/desactiver")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Desactiver(Guid id, CancellationToken ct)
    {
        await _service.DesactiverAsync(id, ct);
        return NoContent();
    }
}
