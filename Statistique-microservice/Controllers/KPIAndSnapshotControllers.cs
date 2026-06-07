using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.Services;
using Statistique_microservice.Domain.Enums;

namespace Statistique_microservice.Controllers;

// ═══════════════════════════════════════════════════════════
// KPIController
// ═══════════════════════════════════════════════════════════
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "MedecinOnly")]
public class KPIController : ControllerBase
{
    private readonly KPIService _service;
    private readonly IValidator<GetKPIsRequest> _validator;

    public KPIController(KPIService service, IValidator<GetKPIsRequest> validator)
    {
        _service   = service;
        _validator = validator;
    }

    // ── GET /api/v1/kpi?cabinetId=&periode=Mensuel&types=... ─
    /// <summary>
    /// Retourne les KPIs calculés pour un cabinet et une période.
    /// Utilise le cache Redis (TTL 1h) pour les calculs récents.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetKPIs(
        [FromQuery] Guid cabinetId,
        [FromQuery] TypePeriode typePeriode = TypePeriode.Mensuel,
        [FromQuery] string? dateDebut = null,
        [FromQuery] string? dateFin = null,
        [FromQuery] TypeKPI[]? types = null,
        CancellationToken ct = default)
    {
        var request = new GetKPIsRequest(
            cabinetId, typePeriode, dateDebut, dateFin, types);

        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _service.GetKPIsAsync(request, ct);
        return Ok(result);
    }

    // ── GET /api/v1/kpi/{cabinetId}/historique/{typeKPI} ─────
    /// <summary>
    /// Historique d'un KPI sur les N dernières périodes.
    /// Utilisé pour les graphiques de tendance.
    /// </summary>
    [HttpGet("{cabinetId:guid}/historique/{typeKPI}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetHistorique(
        Guid cabinetId,
        TypeKPI typeKPI,
        [FromQuery] int nbPeriodes = 12,
        CancellationToken ct = default)
    {
        if (nbPeriodes is < 1 or > 60)
            return BadRequest("nbPeriodes doit être entre 1 et 60.");

        var result = await _service.GetHistoriqueAsync(cabinetId, typeKPI, nbPeriodes, ct);
        return Ok(result);
    }
}

// ═══════════════════════════════════════════════════════════
// SnapshotController
// ═══════════════════════════════════════════════════════════
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Policy = "MedecinOnly")]
public class SnapshotController : ControllerBase
{
    private readonly SnapshotService _service;

    public SnapshotController(SnapshotService service)
        => _service = service;

    // ── GET /api/v1/snapshot?cabinetId=&from=&to= ────────────
    /// <summary>Retourne l'historique des snapshots pour une plage de dates.</summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetHistorique(
        [FromQuery] Guid cabinetId,
        [FromQuery] string from,
        [FromQuery] string to,
        CancellationToken ct)
    {
        if (!DateOnly.TryParse(from, out var dateDebut))
            return BadRequest("Paramètre 'from' invalide — format attendu : yyyy-MM-dd.");

        if (!DateOnly.TryParse(to, out var dateFin))
            return BadRequest("Paramètre 'to' invalide — format attendu : yyyy-MM-dd.");

        if (dateFin < dateDebut)
            return BadRequest("'to' doit être >= 'from'.");

        if (dateFin.DayNumber - dateDebut.DayNumber > 366)
            return BadRequest("La plage maximale est de 366 jours.");

        var result = await _service.GetHistoriqueAsync(
            cabinetId, dateDebut, dateFin, ct);

        return Ok(result);
    }
}
